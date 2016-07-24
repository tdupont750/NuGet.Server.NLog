using System;
using NLog;
using NuGet.Server.Infrastructure;
using NuGet.Server.Publishing;

namespace NuGet.Server.NLog
{
    public class NLogServiceResolver : IServiceResolver
    {
        private readonly IHashProvider _hashProvider;
        private readonly IServerPackageRepository _packageRepository;
        private readonly IPackageAuthenticationService _packageAuthenticationService;
        private readonly IPackageService _packageService;
        private readonly ILogger _logger;
        private readonly Logging.ILogger _loggingLogger;

        public NLogServiceResolver()
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("Initializing NLogServiceResolver");

            var nlogLogger = new NLogLogger(logger);
            _logger = nlogLogger;
            _loggingLogger = nlogLogger;

            _hashProvider = new CryptoHashProvider("SHA512");
            _packageRepository = new ServerPackageRepository(
                PackageUtility.PackagePhysicalPath,
                _hashProvider,
                _loggingLogger)
            {
                // The base class has a separate logger that you have to set.
                Logger = _logger
            };
            
            _packageAuthenticationService = new PackageAuthenticationService();
            _packageService = new PackageService(_packageRepository, _packageAuthenticationService);
        }

        public object Resolve(Type type)
        {
            if (type == typeof(IHashProvider))
                return _hashProvider;

            if (type == typeof(IServerPackageRepository))
                return _packageRepository;

            if (type == typeof(IPackageAuthenticationService))
                return _packageAuthenticationService;

            if (type == typeof(IPackageService))
                return _packageService;

            if (type == typeof(ILogger))
                return _logger;

            if (type == typeof(Logging.ILogger))
                return _loggingLogger;

            return null;
        }
    }
}