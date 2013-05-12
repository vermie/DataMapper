using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMapper
{
    public interface IConvention
    {

    }

    public sealed class AutomaticEnumMappingConvention : IConvention
    {

    }

    public class Conventions
    {
        private List<Type> _conventionList = new List<Type>();

        private List<Type> ConventionList
        {
            get
            {
                return this._conventionList;
            }
        }
        public void Add<TConvention>() where TConvention: IConvention
        {
            var typey = typeof(TConvention);
            if (this.ConventionList.Contains(typey) == false)
            {
                this.ConventionList.Add(typey);
            }
        }  
        public void Remove<TConvention>() where TConvention: IConvention
        {
            var typey = typeof(TConvention);
            if (this.ConventionList.Contains(typey) == false)
            {
                this.ConventionList.Add(typey);
            }
        }
        public Boolean IsDefined<TConvention>() where TConvention : IConvention
        {
            var typey = typeof(TConvention);

            return this.ConventionList.Contains(typey);
        }

        public Conventions()
        {
            this.Add<AutomaticEnumMappingConvention>();
        }
    }
}
