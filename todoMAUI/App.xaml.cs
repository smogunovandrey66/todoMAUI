namespace todoMAUI
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var mainPage = _serviceProvider.GetRequiredService<TodoItemsPage>();
            return new Window(mainPage/*new AppShell()*/);
        }
    }
}