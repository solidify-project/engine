using System.Threading.Tasks;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Infrastructure.Models.Base;

namespace SolidifyProject.Engine.Test.Infrastructure.Services.ContentWriterService
{
    [TestFixture]
    public abstract class _ContentWriterServiceTestBase<T> where T : ModelBase
    {
        protected abstract IContentReaderService<T> ContentReaderService { get; }
        protected abstract IContentWriterService<T> ContentWriterService { get; }

        protected abstract bool AreEqual(T expected, T actual);
        
        [Test]
        public virtual async Task SaveContentAsyncTest(T newElement)
        {
            var newElementActual = await ContentReaderService.LoadContentByIdAsync(newElement.Id);
            
            Assert.IsNull(newElementActual);

            await ContentWriterService.SaveContentAsync(newElement.Id, newElement);
            
            newElementActual = await ContentReaderService.LoadContentByIdAsync(newElement.Id);
         
            Assert.IsTrue(AreEqual(newElement, newElementActual), "Objects are not equal");
        }
    }
}