using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace ScrapporNet
{
    class Fetch
    {
        public Fetch()
        {
            
        }

        public void FetchWinePages()
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load("http://www.saq.com/webapp/wcs/stores/servlet/CatalogSearchResultView?storeId=10001&catalogId=10001&resultCatEntryType=2&beginIndex=0&tri=RechercheUCIProdDescAttributeInfo&sensTri=AscOperator&searchType=100&viewTaskName=SAQCatalogSearchResultView&pageSize=100");
        }
    }
}
