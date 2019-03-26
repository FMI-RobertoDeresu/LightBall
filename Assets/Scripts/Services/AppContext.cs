using Assets.Scripts.ModuleModels.Stage;
using Assets.Scripts.ServiceModels.ConfigServiceModels.Stages;

namespace Assets.Scripts.Services
{
    public class AppContext : SingletonServiceBase<AppContext>
    {
        protected AppContext() { }

        public int LevelToPlay { get; set; }
        public GameOverScreenInfo GameOverInfo { get; set; }

        public static AppContext Create()
        {
            return CreateInstance();
        }
    }
}