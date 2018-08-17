namespace Speercs.Server.Extensibility.Economy {
    public abstract class EconomyResource {
        public abstract string name { get; }
        public abstract int id { get; }
        public virtual int chunk => 10;
    }
}