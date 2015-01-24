using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Cache
{

    public class SystemSpecs
    {

        #region Builder

        public static void Setup(int engineID)
        {
            CacheProvider.Cache.AddItem(ID, engineID);
        }

        #endregion


        #region Private Constructor

        private SystemSpecs() { }

        #endregion


        #region Public Static Properties

        private readonly static string ID = "Gem";
        public static string Username
        {
            get { return CacheProvider.Cache.GetItem(ID) as string; }
        }

        #endregion
        
    }
}
