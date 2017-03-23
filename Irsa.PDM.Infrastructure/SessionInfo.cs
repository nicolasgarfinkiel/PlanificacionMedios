using System.Web;

namespace Irsa.PDM.Infrastructure
{
    public class SessionInfo<T>
        where T : SessionInfo<T>, new()
    {
        private static string Key
        {
            get { return typeof(SessionInfo<T>).FullName; }
        }
 
        private static T Value
        {
            get { return (T) HttpContext.Current.Session[Key]; }
            set { HttpContext.Current.Session[Key] = value; }
        }
 
        public static T Current
        {
            get
            {
                var instance = Value;
               if (instance == null)
                   lock (typeof(T)) 
                  {                     
                       instance = Value;
                       if (instance == null)
                            Value = instance = new T();
                   }
               return instance;
           }
        }
    }

   

}