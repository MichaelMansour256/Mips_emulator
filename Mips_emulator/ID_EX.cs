using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archi_Template
{
    class ID_EX
    {
        public bool Regdest;
        public bool aluop1;
        public bool aluop0;
        public bool aluSrc;
        public bool branch;
        public bool mem_read;
        public bool mem_write;
        public bool reg_write;
        public bool mem_to_reg;

        public uint pc;
        public int read_data1;
        public int read_data2;
        public string address;
        public string rd_reg;
        public string rt_reg;

    }
}
