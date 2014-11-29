using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    using Microsoft.Practices.Unity;

    class Program
    {
        static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();
            // set up the container here 

            IUnitOfWork unitOfWork = container.Resolve<IUnitOfWork>();
            var article = unitOfWork.ArticleRepository.GetById("1");
            article.Name = "New Name";
            unitOfWork.SaveChangesAsync();
        }
    }
}
