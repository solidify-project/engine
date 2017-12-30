namespace SolidifyProject.Engine.Infrastructure.Models.Base
{
    public abstract class ContentModelBase<T> : ModelBase
    {
        public T ContentRaw { get; set; }

        public virtual void Parse()
        {
        }
    }
}