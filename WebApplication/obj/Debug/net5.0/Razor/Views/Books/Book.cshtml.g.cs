#pragma checksum "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6cd5d24f3670b7b4c02432a8aaa92d4331e2da4b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Books_Book), @"mvc.1.0.view", @"/Views/Books/Book.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\_ViewImports.cshtml"
using WebApplication;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\_ViewImports.cshtml"
using WebApplication.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6cd5d24f3670b7b4c02432a8aaa92d4331e2da4b", @"/Views/Books/Book.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c58edd3a6b5e9ca63b10fbb3cbb99bbeb61e4bcd", @"/Views/_ViewImports.cshtml")]
    public class Views_Books_Book : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<BookViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
  
    ViewData["Title"] = "Book Page";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"jumbotron\">\r\n    <h1 class=\"display-3\">Book Edit</h1>\r\n    <hr class=\"my-4\">\r\n</div>\r\n\r\n<div>\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "6cd5d24f3670b7b4c02432a8aaa92d4331e2da4b3916", async() => {
                WriteLiteral("\r\n        <input id=\"Id\" type=\"hidden\" name=\"Id\"");
                BeginWriteAttribute("value", " value=\"", 250, "\"", 267, 1);
#nullable restore
#line 14 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
WriteAttributeValue("", 258, Model.Id, 258, 9, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n\r\n        <label for=\"Name\" class=\"col-form-label float-left\">Book Name:</label>\r\n        <input id=\"Name\" name=\"Name\" type=\"text\" class=\"form-control\" placeholder=\"Book\"");
                BeginWriteAttribute("value", " value=\"", 441, "\"", 460, 1);
#nullable restore
#line 17 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
WriteAttributeValue("", 449, Model.Name, 449, 11, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" required/>
        <div class=""float-left invalid-feedback"">
            Invalid name.
        </div>
        
        <label for=""Authors"" class=""col-form-label float-left"">Authors:</label>
        <div>
            <select id=""Authors"" name=""Authors"" class=""form-control"" multiple>
");
#nullable restore
#line 25 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                 foreach (var author in ViewData["Authors"] as List<AuthorViewModel>)
                {

#line default
#line hidden
#nullable disable
                WriteLiteral("                    ");
                WriteLiteral("<option value=\"");
#nullable restore
#line 27 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                                Write(author.Name);

#line default
#line hidden
#nullable disable
                WriteLiteral("\"\r\n");
#nullable restore
#line 28 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                    if (Model.Authors.Any(a => author.Id == a.Id))
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        ");
                WriteLiteral("selected\r\n");
#nullable restore
#line 31 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                    }

#line default
#line hidden
#nullable disable
                WriteLiteral("                    ");
                WriteLiteral(" >");
#nullable restore
#line 32 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                   Write(author.Name);

#line default
#line hidden
#nullable disable
                WriteLiteral("</option>\r\n");
#nullable restore
#line 33 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                }

#line default
#line hidden
#nullable disable
                WriteLiteral("            </select>\r\n        </div>\r\n\r\n        <label for=\"NumberOfCopies\" class=\"col-form-label float-left\">Number Of Books:</label>\r\n        <input id=\"NumberOfCopies\" name=\"NumberOfCopies\" type=\"number\" class=\"form-control\" min=\"0\"");
                BeginWriteAttribute("value", " value=\"", 1364, "\"", 1393, 1);
#nullable restore
#line 38 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
WriteAttributeValue("", 1372, Model.NumberOfCopies, 1372, 21, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" required/>\r\n        \r\n        <label for=\"Genre\" class=\"col-form-label float-left\">Genre:</label>\r\n        <div>\r\n            <select id=\"Genre\" name=\"Genre\" class=\"form-control\">\r\n");
#nullable restore
#line 43 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                 foreach (var genre in ViewData["Genres"] as List<GenreViewModel>)
                {

#line default
#line hidden
#nullable disable
                WriteLiteral("                    ");
                WriteLiteral(" <option value=\"");
#nullable restore
#line 45 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                                 Write(genre.Id);

#line default
#line hidden
#nullable disable
                WriteLiteral("\"\r\n");
#nullable restore
#line 46 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                     if (Model.Genre != null && Model.Genre.Id == genre.Id)
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        ");
                WriteLiteral(" selected\r\n");
#nullable restore
#line 49 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                    }

#line default
#line hidden
#nullable disable
                WriteLiteral("                    ");
                WriteLiteral(" >");
#nullable restore
#line 50 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                   Write(genre.Name);

#line default
#line hidden
#nullable disable
                WriteLiteral("</option>\r\n");
#nullable restore
#line 51 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                }

#line default
#line hidden
#nullable disable
                WriteLiteral("            </select>\r\n        </div>\r\n\r\n        <button type=\"submit\" class=\"mt-2 btn btn-primary\" value=\"newItem\">Save</button>\r\n\r\n    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</div>\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n    <script>\r\n    $(document).ready(function () {\r\n          $(\'#Authors\').select2({\r\n                multiple: true,\r\n                tags: true\r\n                });\r\n    });\r\n    </script>\r\n");
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<BookViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
