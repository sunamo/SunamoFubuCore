namespace SunamoFubuCore.CommandLine;



public class UsageGraph
{
    private readonly Type _commandType;
    private readonly List<ITokenHandler> _handlers;
    private readonly Type _inputType;
    private readonly IList<CommandUsage> _usages = new List<CommandUsage>();
    private readonly Lazy<IEnumerable<CommandUsage>> _validUsages;

    public UsageGraph(Type commandType)
    {
        _commandType = commandType;
        _inputType = commandType.FindInterfaceThatCloses(typeof(IFubuCommand<>)).GetGenericArguments().First();

        CommandName = CommandFactory.CommandNameFor(commandType);
        _commandType.ForAttribute<CommandDescriptionAttribute>(att => { Description = att.Description; });

        if (Description == null) Description = _commandType.Name;

        _handlers = InputParser.GetHandlers(_inputType);

        _validUsages = new Lazy<IEnumerable<CommandUsage>>(() =>
        {
            if (_usages.Any()) return _usages;

            var usage = new CommandUsage
            {
                Description = Description,
                Arguments = _handlers.OfType<Argument>(),
                ValidFlags = _handlers.Where(x => !(x is Argument))
            };

            return new[] { usage };
        });
    }

    public IEnumerable<ITokenHandler> Handlers => _handlers;

    public string CommandName { get; }

    public IEnumerable<Argument> Arguments => _handlers.OfType<Argument>();

    public IEnumerable<ITokenHandler> Flags
    {
        get { return _handlers.Where(x => !(x is Argument)); }
    }

    public IEnumerable<CommandUsage> Usages => _validUsages.Value;

    public string Description { get; private set; }

    public CommandReport ToReport(string appName)
    {
        return new CommandReport
        {
            Name = CommandName,
            Description = Description,
            Arguments = Arguments.Select(x => x.ToReport()).ToArray(),
            Flags = Flags.Select(x => new FlagReport(x)).ToArray(),
            Usages = Usages.Select(x => x.ToReport(appName, CommandName)).ToArray()
        };
    }

    public object BuildInput(Queue<string> tokens)
    {
        var model = Activator.CreateInstance(_inputType);
        var responding = new List<ITokenHandler>();

        while (tokens.Any())
        {
            var handler = _handlers.FirstOrDefault(h => h.Handle(model, tokens));
            if (handler == null)
                throw new InvalidUsageException("Unknown argument or flag for value " + tokens.Peek());
            responding.Add(handler);
        }

        if (!IsValidUsage(responding)) throw new InvalidUsageException();

        return model;
    }

    public bool IsValidUsage(IEnumerable<ITokenHandler> handlers)
    {
        return _validUsages.Value.Any(x => x.IsValidUsage(handlers));
    }

    public void WriteUsages(string appName)
    {
        if (!Usages.Any())
        {
            CL.WriteLine("No documentation for this command");
            return;
        }

        CL.WriteLine(" Usages for '{0}' ({1})", CommandName, Description);

        if (Usages.Count() == 1)
        {
            CL.ForegroundColor = ConsoleColor.Cyan;
            CL.WriteLine(" " + Usages.Single().ToUsage(appName, CommandName));
            CL.ResetColor();
        }
        else
        {
            writeMultipleUsages(appName);
        }

        if (Arguments.Any())
            writeArguments();


        if (!Flags.Any()) return;

        writeFlags();
    }

    private void writeMultipleUsages(string appName)
    {
        var usageReport = new TwoColumnReport("Usages")
        {
            SecondColumnColor = ConsoleColor.Cyan
        };

        Usages.OrderBy(x => x.Arguments.Count()).ThenBy(x => x.ValidFlags.Count()).Each(u =>
        {
            usageReport.Add(u.Description, u.ToUsage(appName, CommandName));
        });

        usageReport.Write();
    }

    private void writeArguments()
    {
        var argumentReport = new TwoColumnReport("Arguments");
        Arguments.Each(x => argumentReport.Add(x.PropertyName.ToLower(), x.Description));
        argumentReport.Write();
    }

    private void writeFlags()
    {
        var flagReport = new TwoColumnReport("Flags");
        Flags.Each(x => flagReport.Add(x.ToUsageDescription(), x.Description));
        flagReport.Write();
    }

    public UsageExpression<T> AddUsage<T>(string description)
    {
        return new UsageExpression<T>(this, description);
    }

    public CommandUsage FindUsage(string description)
    {
        return _usages.FirstOrDefault(x => x.Description == description);
    }

    public class UsageExpression<T>
    {
        private readonly CommandUsage _commandUsage;
        private readonly UsageGraph _parent;

        public UsageExpression(UsageGraph parent, string description)
        {
            _parent = parent;

            _commandUsage = new CommandUsage
            {
                Description = description,
                Arguments = new Argument[0],
                ValidFlags = _parent.Handlers.Where(x => !x.GetType().CanBeCastTo<Argument>()).ToArray() // Hokum.
            };

            _parent._usages.Add(_commandUsage);
        }


        /// <summary>
        ///     The valid arguments for this command usage in exact order
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public UsageExpression<T> Arguments(params Expression<Func<T, object>>[] properties)
        {
            _commandUsage.Arguments =
            properties.Select(
            expr => _parent.Handlers.FirstOrDefault(x => x.PropertyName == expr.ToAccessor().Name)).OfType
            <Argument>();

            return this;
        }

        /// <summary>
        ///     Optional, use this to limit the flags that are valid with this usage.  If this method is not called,
        ///     the CLI support assumes that every possible flag from the input type is valid
        /// </summary>
        /// <param name="properties"></param>
        public void ValidFlags(params Expression<Func<T, object>>[] properties)
        {
            _commandUsage.ValidFlags =
            properties.Select(
            expr => _parent.Handlers.FirstOrDefault(x => x.PropertyName == expr.ToAccessor().Name))
            .ToArray();
        }
    }
}
