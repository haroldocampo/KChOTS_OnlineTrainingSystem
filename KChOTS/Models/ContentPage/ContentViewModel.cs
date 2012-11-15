using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KChOTS.Models.ContentPage
{
    public class ContentViewModel
    {
        private KnowledgeChannelEntities db;

        public ContentViewModel(KnowledgeChannelEntities knowledgeChannelEntities)
        {
            // TODO: Complete member initialization
            this.db = knowledgeChannelEntities;
        }

        public string Content { get; set; }

        public IEnumerable<SelectListItem> CmsTypes { get; set; }

        internal void PopulateHomeContent()
        {
            Content = db.ContentPages.Where(x => x.ID == 2).SingleOrDefault().PageContent;
        }

        internal void PopulateUpdateView()
        {
            var dbCmsTypes = db.CmsTypes;
            List<SelectListItem> tmpCmsTypes = new List<SelectListItem>();
            foreach (var cmsType in dbCmsTypes) {
                //Do not include the articles in updating
                if (cmsType.Id == 2)
                {
                    continue;
                }
                SelectListItem item = new SelectListItem();
                item.Text = cmsType.CmsType1;
                item.Value = cmsType.Id.ToString();
                tmpCmsTypes.Add(item);
            }
            CmsTypes = tmpCmsTypes;
        }
    }
}