namespace Speercs.Server.Game.Scripting.Api
{
    public class SpeercsUserApi
    {
        public string UserId { get; }

        public SpeercsUserApi(string userId) {
            UserId = userId;
        }
    }
}