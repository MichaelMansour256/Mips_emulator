using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace archi_Template
{
    class pipline
    {
        public string branch_address;
        public string alu_opration;
        public bool pcSrc;
        public bool temp;
        public int[] regs =new int [32];
        public static Dictionary<uint,string> memory_inst = new Dictionary<uint, string>();
        public IF_ID iF_ID = new IF_ID();
        public ID_EX iD_EX = new ID_EX();
        public EX_MEM eX_MEM = new EX_MEM();
        public MEM_WB mEM_WB = new MEM_WB();
        public uint starting_pc;






        public void intialize_memeory_inst(string [] instruction,uint[] pc) {
            memory_inst.Clear();
            for (int i = 0; i < pc.Length; i++) {
                memory_inst.Add(pc[i], instruction[i]);
            }
            int last_pc = pc.Length - 1;
            uint last_pc_value = pc[last_pc];
            //add 5 nop at end of inst mem
            for (int j =0; j<=4;j++) {
                last_pc_value += 4;
                memory_inst.Add(last_pc_value,"00000000000000000000000000000000");
            }
        }
        public void intialize_regs()
        {

            regs[0] = 0;
            for (int i=1; i<32; i++) {
                regs[i] = i + 100;
            }
        }


        public int alu(string aluop, int alu_input_1, int alu_input_2, out int result)
        {
            if (aluop == "01")
            {
                result = Convert.ToInt32(alu_input_1) - Convert.ToInt32(alu_input_2);
                if (result == 0)
                {
                    return 1;
                }
                else {
                    return 0;
                }
                
            }
            else {
                if (alu_opration == "0000")
                {
                    result = Convert.ToInt32(alu_input_1) & Convert.ToInt32(alu_input_2);
                    return 0;
                }
                else if (alu_opration == "0010")
                {
                    result = Convert.ToInt32(alu_input_1) + Convert.ToInt32(alu_input_2);
                    return 0;

                }
                else if (alu_opration == "0110")
                {
                    result = Convert.ToInt32(alu_input_1) - Convert.ToInt32(alu_input_2);
                    if (result == 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else if (alu_opration == "0001")
                {
                    result = Convert.ToInt32(alu_input_1) | Convert.ToInt32(alu_input_2);
                   return  0;
                }
                else {
                    result = 0;
                    return 0;
                }

            }

           
           
        }
        public void alu_control(string funct ,string aluop0,string aluop1) {
            string aluop = aluop1 + aluop0;
            if (aluop=="01") {
                alu_opration = "0110";
            }
            else
            {
                if (funct=="100000") {
                    alu_opration = "0010";

                } else if (funct == "100010") {
                    alu_opration = "0110";

                } else if (funct == "100100") {
                    alu_opration = "0000";

                }
                else {

                    alu_opration = "0001";
                }


            }
        }
        public void control(string inst,out bool Regdest, out bool aluop1,out bool aluop0, out bool aluSrc, out bool branch
            , out bool mem_read, out bool mem_write, out bool reg_write, out bool reg_to_reg) {
            
            
            string opcode = inst.Substring(0,6);
            string funct = inst.Substring(26,6);
            if (opcode == "000000" && (funct == "100000" || funct == "100010" || funct == "100100" || funct == "100101"))
            {
                Regdest = true;
                aluop1 = true;
                aluop0 = false;
                aluSrc = false;
                branch = false;
                mem_read = false;
                mem_write = false;
                reg_write = true;
                reg_to_reg = false;

            }
            else if (opcode == "000100")
            {
                Regdest = false;
                aluop1 = false;
                aluop0 = true;
                aluSrc = false;
                branch = true;
                mem_read = false;
                mem_write = false;
                reg_write = false;
                reg_to_reg = false;

            }
            else {

                Regdest = false;
                aluop1 = false;
                aluop0 = false;
                aluSrc = false;
                branch = false;
                mem_read = false;
                mem_write = false;
                reg_write = false;
                reg_to_reg = false;
            }
           
        }
        public void readreg(string reg_1,string reg_2,out int reg_1_data, out int reg_2_data)
        {
            reg_1= Convert.ToInt32(reg_1, 2).ToString();
            reg_2 = Convert.ToInt32(reg_2, 2).ToString();
            reg_1_data = regs[int.Parse(reg_1)];
            reg_2_data = regs[int.Parse(reg_2)];
        }
        public string mux(string val1,string val2,bool sel)
        {
            if (sel) {
                return val1;
            }
            else {
                return val2;
            }
        }



        public void fetch() {

            starting_pc =pcSrc ? Convert.ToUInt32(branch_address) : starting_pc + 4;
            iF_ID.instruction_code = memory_inst[starting_pc];
            iF_ID.pc = starting_pc + 4;
            pcSrc = temp;

        }
        public void decode()
        {
            string inst = iF_ID.instruction_code;
            string opcode = inst.Substring(0, 6);
            string read_reg1 = inst.Substring(6,5);
            string read_reg2 = inst.Substring(11, 5);
            string write_reg1 = inst.Substring(16, 5);
            string address = inst.Substring(16, 16);
            string funct = inst.Substring(26, 6);
            
            int read_data1;
            int read_data2;
            readreg(read_reg1, read_reg2, out read_data1, out read_data2);
            bool Regdest;
            bool aluop1;
            bool aluop0;
            bool aluSrc;
            bool branch;
            bool mem_read;
            bool mem_write;
            bool reg_write;
            bool mem_to_reg;
            control(inst, out Regdest, out aluop1, out aluop0, out aluSrc, out branch
            , out mem_read, out mem_write, out reg_write, out mem_to_reg);

            iD_EX.read_data1 = read_data1;
            iD_EX.read_data2 = read_data2;
            if (opcode == "000100")
            {
                iD_EX.address = address;
            }
            else {
                iD_EX.address = "0000000000000000";
            }
            iD_EX.pc = iF_ID.pc;
            iD_EX.rt_reg = read_reg2;
            iD_EX.rd_reg = write_reg1;
            
            iD_EX.Regdest = Regdest;
            iD_EX.aluop1 = aluop1;
            iD_EX.aluop0 = aluop0;
            iD_EX.aluSrc = aluSrc;
            iD_EX.branch = branch;
            iD_EX.mem_read = mem_read;
            iD_EX.mem_write = mem_write;
            iD_EX.reg_write = reg_write;
            iD_EX.mem_to_reg = mem_to_reg;
            string aluop0_str = aluop0 ? "1" : "0";
            string aluop1_str = aluop1 ? "1" : "0";
            alu_control(funct, aluop0_str, aluop1_str);

        }
        public void exe()
        {
            int pc_exe = Convert.ToInt32(iD_EX.pc);
            int address =Convert.ToInt32(iD_EX.address,2);
            int address_shifted = address * 4;
            int add_res = address_shifted + pc_exe;
            eX_MEM.adder_result = add_res.ToString();
            eX_MEM.write_data = iD_EX.read_data2;
            string mux_res = mux(iD_EX.rd_reg,iD_EX.rt_reg,iD_EX.Regdest);
            eX_MEM.regDest_mux_result = mux_res;
            ///alu wtf
            string aluop0_str = iD_EX.aluop0 ? "1" : "0";
            string aluop1_str = iD_EX.aluop1 ? "1" : "0";
            string aluop = aluop1_str + aluop0_str;
            int result;
            bool zero;
            int flag= alu(aluop,iD_EX.read_data1, iD_EX.read_data2,out result);
            zero = flag == 1 ? true:false;
            eX_MEM.zero_flag = zero;
            eX_MEM.alu_result = result.ToString();
            eX_MEM.branch = iD_EX.branch;
            eX_MEM.mem_read = iD_EX.mem_read;
            eX_MEM.mem_write = iD_EX.mem_write;
            eX_MEM.mem_to_reg = iD_EX.mem_to_reg;
            eX_MEM.reg_write = iD_EX.reg_write;



        }
        public void mem()
        {
            
            branch_address = eX_MEM.adder_result;
            mEM_WB.mem_to_reg = eX_MEM.mem_to_reg;
            mEM_WB.reg_write = eX_MEM.reg_write;
            mEM_WB.EX_MEM_RegisterRd = eX_MEM.regDest_mux_result;
            mEM_WB.alu_result = eX_MEM.alu_result;
           // pcSrc = eX_MEM.branch & eX_MEM.zero_flag ? true : false;
           temp= eX_MEM.branch & eX_MEM.zero_flag ? true : false;
        }
        public void writeback()
        {
            if (mEM_WB.reg_write) {
               
                int index = Convert.ToInt32(mEM_WB.EX_MEM_RegisterRd,2);
               
                regs[index] = Convert.ToInt32(mEM_WB.alu_result);
            }

        }


    }
}
