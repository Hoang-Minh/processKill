using NUnit.Framework;
using Solution;
using System.Collections.Generic;

namespace processTest
{
    [TestFixture()]
    public class Test
    {
        [Test()]
        public void filterProcessListBy()
        {
            List<string> lsof = new List<string>();
            lsof.Add("syncdefau 629 ramanathanganesan    4u  IPv4 0xb21c42d39c9e749f      0t0  TCP 192.168.1.70:49364->17.248.154.146:https (ESTABLISHED)");
            lsof.Add("mono-sgen 529 ramanathanganesan    4u  IPv4 0xb21c42d39c9e9a9f      0t0  TCP localhost:49270->localhost:49269 (ESTABLISHED)");
            lsof.Add("com.docke 504 ramanathanganesan   23u  IPv4 0xb21c42d398a75f0f      0t0  UDP *:63640");
            lsof.Add("com.docke 504 ramanathanganesan   20u  IPv6 0xb21c42d39752b69f      0t0  TCP localhost:http-alt (LISTEN)");

            List<string> result = Solution.ProcessUtils.filterProcessListBy(processList: lsof, filter: ":6364");

            Assert.NotNull(result);
            Assert.AreEqual(actual: result.Count, expected: 1);
            StringAssert.AreEqualIgnoringCase(actual: result[0], expected: "com.docke 504 ramanathanganesan   23u  IPv4 0xb21c42d398a75f0f      0t0  UDP *:63640");
        }

        [Test()]
        public void filterProcessListByNoMatch()
        {
            List<string> lsof = new List<string>();
            lsof.Add("syncdefau 629 ramanathanganesan    4u  IPv4 0xb21c42d39c9e749f      0t0  TCP 192.168.1.70:49364->17.248.154.146:https (ESTABLISHED)");
            lsof.Add("mono-sgen 529 ramanathanganesan    4u  IPv4 0xb21c42d39c9e9a9f      0t0  TCP localhost:49270->localhost:49269 (ESTABLISHED)");
            lsof.Add("com.docke 504 ramanathanganesan   23u  IPv4 0xb21c42d398a75f0f      0t0  UDP *:63640");
            lsof.Add("com.docke 504 ramanathanganesan   20u  IPv6 0xb21c42d39752b69f      0t0  TCP localhost:http-alt (LISTEN)");

            List<string> result = Solution.ProcessUtils.filterProcessListBy(processList: lsof, filter: ":123");

            Assert.NotNull(result);
            Assert.AreEqual(actual: result.Count, expected: 0);
        }

        [Test()]
        public void filterProcessListByNullProcessList()
        {
            List<string> result = Solution.ProcessUtils.filterProcessListBy(processList: null, filter: ":6364");

            Assert.NotNull(result);
            Assert.AreEqual(actual: result.Count, expected: 0);
        }

        [Test()]
        public void filterProcessListByNullFilter()
        {
            List<string> lsof = new List<string>();
            lsof.Add("syncdefau 629 ramanathanganesan    4u  IPv4 0xb21c42d39c9e749f      0t0  TCP 192.168.1.70:49364->17.248.154.146:https (ESTABLISHED)");
            lsof.Add("mono-sgen 529 ramanathanganesan    4u  IPv4 0xb21c42d39c9e9a9f      0t0  TCP localhost:49270->localhost:49269 (ESTABLISHED)");
            lsof.Add("com.docke 504 ramanathanganesan   23u  IPv4 0xb21c42d398a75f0f      0t0  UDP *:63640");
            lsof.Add("com.docke 504 ramanathanganesan   20u  IPv6 0xb21c42d39752b69f      0t0  TCP localhost:http-alt (LISTEN)");

            List<string> result = Solution.ProcessUtils.filterProcessListBy(processList: lsof, filter: null);

            Assert.NotNull(result);
            Assert.AreEqual(actual: result.Count, expected: 4);
        }

        [Test()]
        public void getOSName()
        {


            Solution.Platform result = Solution.ProcessUtils.getOSName();

            Assert.NotNull(result);
            Assert.AreEqual(actual: result, expected: Solution.Platform.UNIX);
        }

        [Test]
        public void getPidFrom()
        {
            string str = Solution.ProcessUtils.getPidFrom(pidString: "syncdefau 629 ramanathanganesan    4u  IPv4 0xb21c42d39c9e749f      0t0  TCP 192.168.1.70:49364->17.248.154.146:https (ESTABLISHED)",
                                                          pattern: Solution.ProcessUtils.UNIX_PID_REGX);
            Assert.NotNull(str);
            Assert.AreEqual(actual: str, expected: "629");

            str = Solution.ProcessUtils.getPidFrom(pidString: "com.docke 504 ramanathanganesan   20u  IPv6 0xb21c42d39752b69f      0t0  TCP localhost:http-alt (LISTEN)",
                                                          pattern: Solution.ProcessUtils.UNIX_PID_REGX);
            Assert.NotNull(str);
            Assert.AreEqual(actual: str, expected: "504");
            str = Solution.ProcessUtils.getPidFrom(pidString: "TCP    192.168.1.73:56282     23.74.169.78:443       ESTABLISHED     160",
                                                   pattern: Solution.ProcessUtils.WIND_PID_REGX);
            Assert.NotNull(str);
            Assert.AreEqual(actual: str, expected: "160");

        }

        [Test]
        public void splitByLineBreak(){
        
            string input = @"loginwind 115 ramanathanganesan    7u  IPv4 0xb21c42d396d9042f      0t0  UDP *:*
UserEvent 330 ramanathanganesan    3u  IPv4 0xb21c42d396d93267      0t0  UDP *:*
SystemUIS 336 ramanathanganesan   12u  IPv4 0xb21c42d398705737      0t0  UDP *:*";

            List<string> result = Solution.ProcessUtils.splitByLineBreak(processLines: input);

            Assert.NotNull(result);
            Assert.AreEqual(actual: result.Count, expected: 3);
            Assert.AreEqual(actual: result[1], expected: "UserEvent 330 ramanathanganesan    3u  IPv4 0xb21c42d396d93267      0t0  UDP *:*");

        }

        [Test]
        public void splitByLineBreakOneLine()
        {

            string input = @"loginwind 115 ramanathanganesan    7u  IPv4 0xb21c42d396d9042f      0t0  UDP *:*";

            List<string> result = Solution.ProcessUtils.splitByLineBreak(processLines: input);

            Assert.NotNull(result);
            Assert.AreEqual(actual: result.Count, expected: 1);
            Assert.AreEqual(actual: result[0], expected: "loginwind 115 ramanathanganesan    7u  IPv4 0xb21c42d396d9042f      0t0  UDP *:*");

        }

        [Test]
        public void splitByLineBreakNullLine()
        {

            List<string> result = Solution.ProcessUtils.splitByLineBreak(processLines: null);

            Assert.NotNull(result);
            Assert.AreEqual(actual: result.Count, expected: 0);
        }

        [Test]
        public void splitByLineBreakEmptyLine()
        {

            List<string> result = Solution.ProcessUtils.splitByLineBreak(processLines: "");

            Assert.NotNull(result);
            Assert.AreEqual(actual: result.Count, expected: 0);
        }
    }
}
