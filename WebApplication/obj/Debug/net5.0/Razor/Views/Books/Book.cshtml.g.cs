#pragma checksum "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d98fdd6c69d1eca42ef6243857998c13dadcd5a5"
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d98fdd6c69d1eca42ef6243857998c13dadcd5a5", @"/Views/Books/Book.cshtml")]
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
            WriteLiteral("\r\n<div class=\"jumbotron\">\r\n    <h1 class=\"display-3\">Book Edit</h1>\r\n    <hr class=\"my-4\">\r\n");
            WriteLiteral("</div>\r\n\r\n<div>\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d98fdd6c69d1eca42ef6243857998c13dadcd5a53947", async() => {
                WriteLiteral("\r\n        <input id=\"Id\" type=\"hidden\" name=\"Id\"");
                BeginWriteAttribute("value", " value=\"", 371, "\"", 388, 1);
#nullable restore
#line 17 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
WriteAttributeValue("", 379, Model.Id, 379, 9, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n\r\n        <label for=\"Name\" class=\"col-form-label float-left\">Book Name:</label>\r\n        <input id=\"Name\" name=\"Name\" type=\"text\" class=\"form-control\" placeholder=\"Book\"");
                BeginWriteAttribute("value", " value=\"", 562, "\"", 581, 1);
#nullable restore
#line 20 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
WriteAttributeValue("", 570, Model.Name, 570, 11, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" required/>
        <div class=""float-left invalid-feedback"">
            Invalid name.
        </div>
        <label for=""Authors"" class=""form-label"">Authors:</label>

        <div>
            <select id=""Authors"" name=""Authors"" class=""form-control"" multiple>
");
#nullable restore
#line 28 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                 foreach (var author in ViewData["Authors"] as List<AuthorViewModel>)
                {

#line default
#line hidden
#nullable disable
                WriteLiteral("                    ");
                WriteLiteral("<option value=\"");
#nullable restore
#line 30 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                                Write(author.Name);

#line default
#line hidden
#nullable disable
                WriteLiteral("\"\r\n");
#nullable restore
#line 31 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                    if (Model.Authors.Any(a => author.Id == a.Id))
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        ");
                WriteLiteral("selected\r\n");
#nullable restore
#line 34 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                    }

#line default
#line hidden
#nullable disable
                WriteLiteral("                    ");
                WriteLiteral(" >");
#nullable restore
#line 35 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                   Write(author.Name);

#line default
#line hidden
#nullable disable
                WriteLiteral("</option>\r\n");
#nullable restore
#line 36 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                }

#line default
#line hidden
#nullable disable
                WriteLiteral(@"            </select>
        </div>

        <label for=""NumberOfCopies"" class=""col-form-label float-left"">Number Of Books:</label>
        <input id=""NumberOfCopies"" name=""NumberOfCopies"" type=""number"" class=""form-control"" min=""0"" required/>

        <div>
            <select id=""Genre"" name=""Genre"" class=""form-control"">
");
#nullable restore
#line 45 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                 foreach (var genre in ViewData["Genres"] as List<GenreViewModel>)
                {

#line default
#line hidden
#nullable disable
                WriteLiteral("                    ");
                WriteLiteral(" <option value=\"");
#nullable restore
#line 47 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                                 Write(genre.Id);

#line default
#line hidden
#nullable disable
                WriteLiteral("\"\r\n");
#nullable restore
#line 48 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                     if (Model.Genre.Id == genre.Id)
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        ");
                WriteLiteral(" selected\r\n");
#nullable restore
#line 51 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                    }

#line default
#line hidden
#nullable disable
                WriteLiteral("                    ");
                WriteLiteral(" >");
#nullable restore
#line 52 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                   Write(genre.Name);

#line default
#line hidden
#nullable disable
                WriteLiteral("</option>\r\n");
#nullable restore
#line 53 "C:\Users\djliz\RiderProjects\TheLibraryOfBabel\WebApplication\Views\Books\Book.cshtml"
                }

#line default
#line hidden
#nullable disable
                WriteLiteral("            </select>\r\n        </div>\r\n\r\n        <button type=\"submit\" class=\"btn btn-primary\" value=\"newItem\">Save</button>\r\n\r\n    ");
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
                WriteLiteral(@"
    <script>
    $(document).ready(function () {
          $('#Authors').select2({
                // dropdownParent: $(""#modals-body""),
                multiple: true,
                tags: true
                // width: '100%',
                // theme: 'bootstrap4'
                //    placeholder: 'Categories',
                });
    });
</script>
");
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
