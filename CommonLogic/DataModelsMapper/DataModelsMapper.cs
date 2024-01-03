using System.Reflection;

namespace CommonLogic.DataModelsMapper
{
    public static class DataModelsMapper
    {
        public static TDestinationObj Mapp<TSourceObj, TDestinationObj>(TSourceObj sourceObj, TDestinationObj destinationObj)
        {
            return AutoMapp(sourceObj, destinationObj);
        }

        private static TDestinationObj AutoMapp<TSourceObj, TDestinationObj>(TSourceObj sourceObj, TDestinationObj destinationObj)
        {
            foreach (var sourceProp in sourceObj.GetType().GetProperties())
            {
                PropertyInfo destinationProp = typeof(TDestinationObj).GetProperty(sourceProp.Name);

                if(destinationProp != null)
                {
                    destinationProp.SetValue(destinationObj, sourceProp.GetValue(sourceObj, null));
                }
            }
            return destinationObj;
        }
    }
}
