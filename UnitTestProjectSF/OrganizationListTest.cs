using Moq;
using System.Collections.Generic;
using NUnit.Framework;
using RegionR.Views.Abstract;
using RegionReport.Domain.SF.Abstract;
using NSubstitute;
using RegionalR.Presentation.Concrete;
using ClassLibrary.SF.Entities;
using RegionReport.Domain;
using System.Linq;

namespace UnitTestProjectSF
{
    [TestFixture]
    public class OrganizationListTest
    {
        private IOrganizationListView view;
        private IOrganizationRepository repository;

        [SetUp]
        public void SetUp()
        {
            view = Substitute.For<IOrganizationListView>();
            
            repository = Substitute.For<IOrganizationRepository>();
            repository.Organizations.Returns(new Organization[] {
                new Organization(TypeOrg.ЛПУ) {ID = 1},
                new Organization(TypeOrg.ЛПУ) {ID = 2},
                new Organization(TypeOrg.ЛПУ) {ID = 3}
            }.AsQueryable());

            var presenter = new OrganizationListPresenter(view, repository);
            presenter.Run();
        }

        [Test]
        public void Count_Is_3()
        {
            int count = view.Organizations.Count();
            
            Assert.AreEqual(3, count);
        }
    }
}
