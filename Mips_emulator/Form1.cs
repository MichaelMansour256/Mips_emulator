using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace archi_Template
{
    public partial class Form1 : Form
    {
        pipline p = new pipline();
        public Form1()
        {
            
            InitializeComponent();
        }

        private void inializeBtn_Click(object sender, EventArgs e)
        {
            //flush pipline regs
            flush();
            PiplineGrid.Rows.Clear();
            MipsRegisterGrid.Rows.Clear();
            MipsRegisterGrid.Refresh();
            p.starting_pc = Convert.ToUInt32(StartPCTxt.Text)-4;
            p.intialize_regs();
            for (int i=0;i<p.regs.Length;i++) {
                MipsRegisterGrid.Rows.Add(i,p.regs[i]);
            }
            string machine_code =UserCodetxt.Text;
            string [] code_line=machine_code.Split('\n');
            int len = code_line.Length;
            
            uint[] pc = new uint[len];
            string[] inst = new string[len];
            for (int i = 0; i < len; i++)
            {
                pc[i]= Convert.ToUInt32(code_line[i].Substring(0,4));
            }
            for (int i = 0; i < len; i++)
            {
                inst[i] = code_line[i].Substring(5);
                inst[i]=inst[i].Replace(":"," ");
                inst[i]=inst[i].Replace(" ", String.Empty);
            }
            
            p.intialize_memeory_inst(inst,pc);
           
           
        }

        private void runCycleBtn_Click(object sender, EventArgs e)
        {

           
            if (!p.pcSrc) {
                p.writeback();
                p.mem();
                p.exe();
                p.decode();
                p.fetch();
                display();
            }
            else{
                flush();
                p.fetch();
                display();
                p.pcSrc = false;
                p.temp = false;
            }
            


        }
        void flush() {
            //  if / id
           
            p.iF_ID.pc = 0;
            p.iF_ID.instruction_code = "00000000000000000000000000000000";
            //  id / exe
            p.iD_EX.Regdest = false;
            p.iD_EX.aluop1 = false;
            p.iD_EX.aluop0= false;
            p.iD_EX.aluSrc = false;
            p.iD_EX.branch = false;
            p.iD_EX.mem_read = false;
            p.iD_EX.mem_write = false;
            p.iD_EX.reg_write = false;
            p.iD_EX.mem_to_reg = false;
            p.iD_EX.pc = 0;
            p.iD_EX.read_data1 = 0;
            p.iD_EX.read_data2 = 0;
            p.iD_EX.address = "0000000000000000";
            p.iD_EX.rd_reg = "00000";
            p.iD_EX.rt_reg = "00000";

            // exe / mem
            p.eX_MEM.branch = false;
            p.eX_MEM.mem_read = false;
            p.eX_MEM.mem_write = false;
            p.eX_MEM.reg_write = false;
            p.eX_MEM.mem_to_reg = false;
            p.eX_MEM.adder_result = "0000";
            p.eX_MEM.alu_result = "0";
            p.eX_MEM.zero_flag = false;
            p.eX_MEM.write_data = 0;
            p.eX_MEM.regDest_mux_result = "00000";

            // mem / wb
            p.mEM_WB.reg_write = false;
            p.mEM_WB.mem_to_reg = false;
            p.mEM_WB.read_data = "0";
            p.mEM_WB.alu_result = "0";
            p.mEM_WB.EX_MEM_RegisterRd = "00000";
            //p.pcSrc = false;

        }
        void display()
        {

            MipsRegisterGrid.Rows.Clear();
            MipsRegisterGrid.Refresh();
            for (int i = 0; i < p.regs.Length; i++)
            {
                MipsRegisterGrid.Rows.Add(i, p.regs[i]);
            }
            PiplineGrid.Rows.Clear();
            PiplineGrid.Refresh();
            //  if / id
            PiplineGrid.Rows.Add("IF/ID");
            PiplineGrid.Rows.Add("pc", p.iF_ID.pc);
            PiplineGrid.Rows.Add("insttruction code", p.iF_ID.instruction_code);


            //   id/ex
            PiplineGrid.Rows.Add("ID/EX");
            PiplineGrid.Rows.Add("regdest", p.iD_EX.Regdest);
            PiplineGrid.Rows.Add("aluop1", p.iD_EX.aluop1);
            PiplineGrid.Rows.Add("aluop0", p.iD_EX.aluop0);
            PiplineGrid.Rows.Add("aluSrc", p.iD_EX.aluSrc);
            PiplineGrid.Rows.Add("branch", p.iD_EX.branch);
            PiplineGrid.Rows.Add("mem read", p.iD_EX.mem_read);
            PiplineGrid.Rows.Add("mem write", p.iD_EX.mem_write);
            PiplineGrid.Rows.Add("reg write", p.iD_EX.reg_write);
            PiplineGrid.Rows.Add("mem to reg", p.iD_EX.mem_to_reg);
            PiplineGrid.Rows.Add("pc", p.iD_EX.pc);
            PiplineGrid.Rows.Add("reg 1 data", p.iD_EX.read_data1);
            PiplineGrid.Rows.Add("reg 2 data", p.iD_EX.read_data2);
            PiplineGrid.Rows.Add("address", p.iD_EX.address);
            PiplineGrid.Rows.Add("rd reg", p.iD_EX.rd_reg);
            PiplineGrid.Rows.Add("rt reg", p.iD_EX.rt_reg);

            //  ex/mem
            PiplineGrid.Rows.Add("EX/MEM");
            PiplineGrid.Rows.Add("branch", p.eX_MEM.branch);
            PiplineGrid.Rows.Add("mem read", p.eX_MEM.mem_read);
            PiplineGrid.Rows.Add("mem write", p.eX_MEM.mem_write);
            PiplineGrid.Rows.Add("reg write", p.eX_MEM.reg_write);
            PiplineGrid.Rows.Add("mem to reg", p.eX_MEM.mem_to_reg);
            PiplineGrid.Rows.Add("adder_result", p.eX_MEM.adder_result);
            PiplineGrid.Rows.Add("alu_result", p.eX_MEM.alu_result);
            PiplineGrid.Rows.Add("zero_flag", p.eX_MEM.zero_flag);
            PiplineGrid.Rows.Add("write_data", p.eX_MEM.write_data);
            PiplineGrid.Rows.Add("regDest_mux_result", p.eX_MEM.regDest_mux_result);

            // mem/wb
            PiplineGrid.Rows.Add("MEM/WB");
            PiplineGrid.Rows.Add("reg write", p.mEM_WB.reg_write);
            PiplineGrid.Rows.Add("mem to reg", p.mEM_WB.mem_to_reg);
            PiplineGrid.Rows.Add("read_data", p.mEM_WB.read_data);
            PiplineGrid.Rows.Add("alu_result", p.mEM_WB.alu_result);
            PiplineGrid.Rows.Add("EX_MEM_RegisterRd", p.mEM_WB.EX_MEM_RegisterRd);
            

        }
    }
}
