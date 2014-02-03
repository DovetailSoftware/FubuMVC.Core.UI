using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using FubuMVC.Core.Http;
using FubuMVC.Core.UI.Testing.Elements;
using FubuMVC.Core.Urls;
using FubuMVC.Core.View;
using FubuTestingSupport;
using HtmlTags;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Core.UI.Testing.Forms
{
    [TestFixture]
    public class FormForExpressionTester
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            page = MockRepository.GenerateMock<IFubuPage>();
        }

        #endregion

        private IFubuPage page;

        public class AddressController
        {
            public AddressViewModel Address()
            {
                return new AddressViewModel();
            }
        }

        [Test]
        public void basic_form_for()
        {
            page.FormFor().ShouldNotBeNull().ShouldBeOfType<FormTag>();
        }

        [Test]
        public void end_form()
        {
            page.EndForm().ToString().ShouldEqual("</form>");
        }


        [Test]
        public void form_for_with_an_url()
        {
            page.Stub(x => x.Get<ICurrentHttpRequest>()).Return(new StubCurrentHttpRequest
            {
                TheApplicationRoot = "http://server"
            });

            page.FormFor("some action").Attr("action").ShouldEqual("http://server/some action");
        }

    }

    public class StubCurrentHttpRequest : ICurrentHttpRequest
    {
        public string TheRawUrl;
        public string TheRelativeUrl;
        public string TheApplicationRoot = "http://server";
        public string TheHttpMethod = "GET";
        public string StubFullUrl = "http://server/";

        public string RawUrl()
        {
            return TheRawUrl;
        }

        public string RelativeUrl()
        {
            return TheRelativeUrl;
        }

        public string FullUrl()
        {
            return StubFullUrl;
        }

        public string ToFullUrl(string url)
        {
            return url.ToAbsoluteUrl(TheApplicationRoot);
        }

        public string HttpMethod()
        {
            return TheHttpMethod;
        }

        public bool HasHeader(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetHeader(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> AllHeaderKeys()
        {
            throw new NotImplementedException();
        }

        public NameValueCollection QueryString { get; private set; }
    }
}