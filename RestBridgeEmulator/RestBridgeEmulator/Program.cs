namespace RestBridgeEmulator
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using System.ServiceModel.Web;
    using Emulator;
    using RestContract;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Инициализация...");
            var properties = ConnectionSettings.Default;
            var baseUrl = new Uri(properties.host);

            var contractType = typeof (IRestBridgeContract);
            var binding = new WebHttpBinding(WebHttpSecurityMode.None);
            binding.ContentTypeMapper = new JsonContentTypeMapper();

            var server = new ServiceHost(typeof (CastDriverBridgeEmulatorImpl));
            var endPoint = server.AddServiceEndpoint(contractType, binding, baseUrl);

            var behavior = new WebHttpBehavior();
            behavior.FaultExceptionEnabled = false;
            behavior.HelpEnabled = true;
            behavior.DefaultOutgoingRequestFormat = WebMessageFormat.Json;
            behavior.DefaultOutgoingResponseFormat = WebMessageFormat.Json;

            endPoint.Behaviors.Add(behavior);

            Console.WriteLine("Инициализация завершена.");

            Console.WriteLine("Запуск сервера...");
            server.Open();
            Console.WriteLine("Сервер запущен. Адрес сервера: {0}\nДля остановки нажмите Enter.", properties.host);

            Console.ReadLine();
            Console.WriteLine("Остановка сервера...");
            server.Close();
            Console.WriteLine("Сервер остановлен.");
            Console.ReadLine();
        }
    }
}
