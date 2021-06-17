using AutoBrowser.Actions;
using AutoBrowser.Classes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoBrowser
{
    public partial class Tester : Form
    {
        #region Global Variables
        private AutoWebBrowser autoWeb;
        private bool IsAuto = false;
        #endregion

        #region Properties
        public string FileName { get; set; }
        #endregion

        #region Constructor
        public Tester()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                List<BaseAction> steps = new List<BaseAction>
                {
                    //new Repeat("pagesIndex",1,"[pagesIndex]+1",new List<BaseAction>(){
                    //    new Repeat("figureIndex",12,new List<BaseAction>(){
                    //    new Redirect("https://www.xinmeitulu.com/tag/cosplay/page/[pagesIndex]",30), 
                    //    //https://www.xinmeitulu.com/tag/hushizhifu/page/3  => nurse 26
                    //    //https://www.xinmeitulu.com/tag/nyupu/page/3 => maid 56  continue in the page 9
                    //    //https://www.xinmeitulu.com/tag/tunyulang/page/3 => bunnygirl 37
                    //    //https://www.xinmeitulu.com/tag/leisiyouhuo/page/3  lace tentation 17  continue process in page 3
                    //    //https://www.xinmeitulu.com/tag/baisi/page/3  best 44
                    //    //https://www.xinmeitulu.com/tag/heisimeitui/page/3  black silk leng 51
                    //    //https://www.xinmeitulu.com/tag/siwashaofu   stockings 26
                    //    //https://www.xinmeitulu.com/tag/chuangshang   bed 66
                    //    //https://www.xinmeitulu.com/tag/heisiyouhuo black lace tentation 30

                    //        new ExtractElement("a",  new List<Node>(){
                    //            new MultiNode(HtmlTag.figure,"[figureIndex]"),
                    //            new MultiNode(HtmlTag.a, 0)
                    //        }),
                    //        new ExtractAttribute("postLink", "a", HtmlAttribute.href),
                    //        new ExtractElement("img",  new List<Node>(){
                    //            new MultiNode(HtmlTag.figure, "[figureIndex]"),
                    //            new MultiNode(HtmlTag.img, 0)
                    //        }),
                    //        new ExtractAttribute("folder","img",HtmlAttribute.alt),

                    //        new Redirect("[postLink]",30),

                    //        new ExtractElement("img",  new List<Node>(){
                    //            new MultiNode(HtmlTag.figure),
                    //        }),
                    //        new ExtractAttribute("imageCount","img",HtmlAttribute.length),
                    //        new ToastNotification("Downloading Gallery [folder]"),
                    //        new Repeat("imageIndex","[imageCount]",new List<BaseAction>(){
                    //            new ExtractElement("img", new List<Node>(){
                    //                new MultiNode(HtmlTag.img,"[imageIndex]")
                    //            }),
                    //            new ExtractAttribute("url","img","src"),
                    //            new ExtractAttribute("title","img","alt"),
                    //            new Download("[url]","[title].jpg","Cosplay/[folder]")
                    //        })
                    //    })
                    //}),
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //new Repeat("i",1,"[i]+300", new List<BaseAction>(){
                    //    new Redirect("https://jav.guru/page/[i]/"),
                    //    new Repeat("j",24,new List<BaseAction>(){
                    //        new ExtractElement("img",new List<Node>()
                    //        {
                    //            new SingleNode(SingleNode.SingleNodeType.Id,"main"),
                    //            new MultiNode(Enums.HtmlTag.img,"[j]")
                    //        }),
                    //        new ExtractAttribute("imgLink","img","src"),
                    //        new ExtractAttribute("imgName","img","alt"),
                    //        new Download("[imgLink]","[imgName].jpg")
                    //    })
                    //})
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //new Repeat("pageIndex",1,"[pageIndex]+2", new List<BaseAction>(){
                    //    //new Redirect("https://javhd.today/recent/[pageIndex]/"),
                    //    //new Redirect("https://javhd.today/search/video/?s=bride&page=[pageIndex]"),
                    //    //new Redirect("https://javhd.today/search/video/?s=maid&page=[pageIndex]"),//1-7
                    //    //new Redirect("https://javhd.today/search/video/?s=yua+Mikami"),
                    //    new Redirect("https://javhd.today/search/video/?s=bukkake"),
                    //    new ExtractElement("modal",new List<Node>()
                    //    {
                    //        new MultiNode(HtmlTag.div,"modal-body","")
                    //    }),
                    //  new ExtractAttribute("modalCount","modal",HtmlAttribute.length),
                    //    new Repeat("modalIndex","[modalCount]",new List<BaseAction>(){
                    //        new ExtractElement("modal",new List<Node>()
                    //        {
                    //            new MultiNode(HtmlTag.div,"modal-body","[modalIndex]"),
                    //            new MultiNode(HtmlTag.a,0)
                    //        }),
                    //        new ExtractAttribute("title","modal",HtmlAttribute.title),

                    //        new ExtractElement("modal",new List<Node>()
                    //        {
                    //            new MultiNode(HtmlTag.div,"modal-body","[modalIndex]"),
                    //            new MultiNode(HtmlTag.video,0)
                    //        }),
                    //        new ExtractAttribute("posterLink","modal",HtmlAttribute.poster),
                    //        new Download("[posterLink]","[title].jpg","javhd.today"),

                    //        new ExtractElement("modal",new List<Node>()
                    //        {
                    //            new MultiNode(HtmlTag.div,"modal-body","[modalIndex]"),
                    //            new MultiNode(HtmlTag.source,0)
                    //        }),
                    //        new ExtractAttribute("videoLink","modal",HtmlAttribute.src),
                    //        new Download("[videoLink]","[title].mp4","javhd.today")
                    //    })
                    //})
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //new Redirect("https://monoschinos2.com/",20),
                    //new Repeat("capIndex",24,new List<BaseAction>(){
                    //    new ExtractElement("cap",new List<Node>(){
                    //        new MultiNode(HtmlTag.section,"caps",0),
                    //        new MultiNode(HtmlTag.h2,"[capIndex]"),
                    //    }),
                    //    new ExtractAttribute("capName","cap","innerText"),

                    //    new ExtractElement("cap",new List<Node>(){
                    //        new MultiNode(HtmlTag.section,"caps",0),
                    //        new MultiNode(HtmlTag.div,"dataEpi","[capIndex]"),
                    //    }),
                    //    new ExtractAttribute("capNo","cap","innerText"),

                    //    new Conditional("[capName]==Ijiranaide, Nagatoro-san",new List<BaseAction>()
                    //    {
                    //        new ToastNotification("New Chapter of [capName] No.[capNo]")
                    //    })
                    //}),
                    new Input("topic","Topic"){ Format= Input.SpecialFormat.URLEncoding },
                    new Redirect("https://www.google.com/search?client=firefox-b-d&q=[topic]"),
                    new ExtractElement("urlEl",new List<Node>(){
                        new MultiNode(Enums.HtmlTag.div,"g",18),
                        new MultiNode(Enums.HtmlTag.a,0)
                    }),
                    new ExtractAttribute("url","urlEl",Enums.HtmlAttribute.href),
                    new Redirect("[url]"),
                    new ExtractElement("codeEl",new List<Node>(){
                        new MultiNode(Enums.HtmlTag.code,0),
                    }),
                    new ExtractAttribute("code","codeEl","text"),
                    new WriteFile("[code]","code.txt")
                };

                //string file = "monoschinos.aweb";
                //new AutoBrowser.Classes.Project().SaveProject(steps, file);
                //List<BaseAction> result = new AutoBrowser.Classes.Project().LoadProject(file);

                autoWeb.Run(steps);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmTEST_Shown(object sender, EventArgs e)
        {
            IsAuto = !string.IsNullOrEmpty(FileName);

            autoWeb = new AutoWebBrowser(formBrowser);
            autoWeb.ProgressChanged += (s, ev) => this.Text = ev.Description;
            autoWeb.ProcessFinished += (s, ev) =>
            {
                if (IsAuto) { Environment.Exit(0); }
                #endregion
                MessageBox.Show("Process Finished", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            if (IsAuto)
            {
                btnStart.Visible = false;
                List<BaseAction> actions = new Project().LoadProject(FileName);
                autoWeb.Run(actions);
            }
        }
    }
}
