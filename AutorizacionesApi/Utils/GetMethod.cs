using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WebApi.Utils
{
    public class GetMethod
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string Current()
        {
            StackTrace st = new StackTrace();
            //StackFrame sf = st.GetFrame(1);
            StackFrame sf = st.GetFrame(3);
            return sf.GetMethod().Name;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int ErrorLine(Exception ex)
        {
            //var st = new StackTrace(ex, true);
            //var frame = st.GetFrame(0);
            //// Get the line number from the stack frame
            //return frame.GetFileLineNumber();

            var trace = new StackTrace(ex, true);
            var frame = trace.GetFrames().Last();
            var lineNumber = frame.GetFileLineNumber();
            return lineNumber;
        }

        public static string GetMethodName([CallerMemberName] string memberName = "")
        {
            return memberName;
        }

        //public static string TraceMessage(string message,
        //[CallerMemberName] string memberName = "",
        //[CallerFilePath] string sourceFilePath = "",
        //[CallerLineNumber] int sourceLineNumber = 0)
        //{
        //    var output = message + "; " + memberName + "; " + sourceFilePath + "; " + sourceLineNumber;
        //    return output;
        //}

    }
}
