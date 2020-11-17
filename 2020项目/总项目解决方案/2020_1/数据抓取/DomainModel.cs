using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace 数据抓取
{
    public class DomainModel
    {
        // Token: 0x1700005E RID: 94
        // (get) Token: 0x060001DA RID: 474 RVA: 0x000153F5 File Offset: 0x000135F5
        // (set) Token: 0x060001DB RID: 475 RVA: 0x000153FD File Offset: 0x000135FD
        public string Domain { get; set; }

        // Token: 0x1700005F RID: 95
        // (get) Token: 0x060001DC RID: 476 RVA: 0x00015406 File Offset: 0x00013606
        // (set) Token: 0x060001DD RID: 477 RVA: 0x0001540E File Offset: 0x0001360E
        public string Cookies { get; set; }

        // Token: 0x17000060 RID: 96
        // (get) Token: 0x060001DE RID: 478 RVA: 0x00015417 File Offset: 0x00013617
        // (set) Token: 0x060001DF RID: 479 RVA: 0x0001541F File Offset: 0x0001361F
        public string Proxip { get; set; }

        // Token: 0x17000061 RID: 97
        // (get) Token: 0x060001E0 RID: 480 RVA: 0x00015428 File Offset: 0x00013628
        // (set) Token: 0x060001E1 RID: 481 RVA: 0x00015430 File Offset: 0x00013630
        public WebHeaderCollection Heard { get; set; }
    }
}
