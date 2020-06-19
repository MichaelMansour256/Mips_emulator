using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archi_Template
{
    class EX_MEM
    {

        public bool branch;
        public bool mem_read;
        public bool mem_write;
        public bool reg_write;
        public bool mem_to_reg;
        public string adder_result="0";
        public string alu_result="0";
        public bool zero_flag;
        public int write_data;
        public string regDest_mux_result="0";

    }
}
