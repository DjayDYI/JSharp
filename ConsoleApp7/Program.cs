// See https://aka.ms/new-console-template for more information

// See https://aka.ms/new-console-template for more information


using System.Net.Sockets;

class Program
{
    class JStack
    {
        private String[] program;
        private Dictionary<String, int> label = new Dictionary<string, int>();
        private Dictionary<String, String> token = new Dictionary<string, string>();
        private Stack<String> stack = new Stack<string>();
        private int pc = 0;
        private bool running = true;
        
        public JStack(String program)
        {
            this.program = program.Split(" ");
            foreach (var command in this.program)
            {
                if (command.Contains(":"))
                    label.Add(command.Replace(":", ""), pc);
                pc++;
            }
            pc = 0;
        }

        public void run()
        {
            while (pc < program.Length && running)
            {
                if(program[pc] == "halt") halt();
                else if(program[pc] == "push") push();
                else if(program[pc] == "pop") pop();
                else if(program[pc] == "add") add();
                else if(program[pc] == "sub") sub();
                else if(program[pc] == "mul") mul();
                else if(program[pc] == "div") div();
                else if(program[pc] == "gt") greater();
                else if(program[pc] == "lt") lower();
                else if(program[pc] == "eq") equals();
                else if(program[pc] == "neq") notEquals();
                else if(program[pc] == "not") not();
                else if(program[pc] == "print") print();
                else if(program[pc] == "mov") mov();
                else if(program[pc] == "load") load();
                else if(program[pc] == "branch") branch();
                else if(program[pc] == "") skip();
                else if(program[pc].Contains(":")) skip();
                else error();
                pc++;
            }
        }

        void skip()
        {
        }

        void push()
        {
            stack.Push(program[++pc]);
        }

        string pop()
        {
            String str = stack.Pop();
            if (str == "null")
            {
                error();
                return "null";
            }
            else
            {
                return str;
            }
        }

        void error()
        {
            Console.WriteLine("Error command does not exist");
            halt();
        }

        void halt()
        {
            Console.WriteLine("Halting program...");
            running = false;
        }

        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }
        
        
        void add()
        {
            String rval = pop();
            String lval = pop();
            if (!IsNumeric(rval))
                rval = token[rval];    
            if (!IsNumeric(lval))
                lval = token[lval]; 
            String res = (int.Parse(rval) + int.Parse(lval)).ToString();
            stack.Push(res);
        }
        
        void sub()
        {
            String rval = pop();
            String lval = pop();
            String res = (int.Parse(rval) - int.Parse(lval)).ToString();
            stack.Push(res);
        }
        
        void mul()
        {
            String rval = pop();
            String lval = pop();
            String res = (int.Parse(rval) * int.Parse(lval)).ToString();
            stack.Push(res);
        }

        void div()
        {
            String rval = pop();
            String lval = pop();
            String res = (int.Parse(rval) / int.Parse(lval)).ToString();
            stack.Push(res);
        }
        
        void lower()
        {
            String rval = pop();
            String lval = pop();
            if (int.Parse(rval) < int.Parse(lval))
                stack.Push("true");
            else
               stack.Push("false");
        }
        
        void greater()
        {
            String rval = pop();
            String lval = pop();
            if (int.Parse(rval) > int.Parse(lval))
                stack.Push("true");
            else
                stack.Push("false");
        }

        void equals()
        {
            String rval = pop();
            String lval = pop();
            if (rval == lval)
                stack.Push("true");
            else
                stack.Push("false");
        }
        
        void not()
        {
            String rval = pop();
            if (rval == "true")
                stack.Push("false");
            else
                stack.Push("true");
        }

        void notEquals()
        {
            equals();
            not();
        }

        void print()
        {
            String peek = pop();
            Console.WriteLine(peek);
            stack.Push(peek);
        }

        void mov()
        {
            String name = pop();
            String val = pop();
            if (token.ContainsKey(name))
                token[name] = val;
            else
                token.Add(name, val);
            stack.Push(name);
        }
        
        void load()
        {
            String name = pop();
            if (token.ContainsKey(name))
                stack.Push(token[name]);
            else
                stack.Push("null");
        }

        void branch()
        {
            if (pop() == "true")
                pc = label[program[++pc]];
            else
                ++pc;
        }
    
    }
    
    public static void Main(String[] args)
    {
        string code = File.ReadAllLines(args[0]).ToString();
        Assembly assemblem = new Assembly("..\\"+code);
        JStack machine2 = new JStack(assemblem.Parse());
        machine2.run();
    }
}

public class Assembly
{
    private string assembly = "";
    private String[] command;
    private int i;
    private int forN = 0;
    private int ifN = 0;
    
    public Assembly(String program)
    {
        command = program.Split(" ");
        for (i=0; i<command.Length; i++)
        {
            switch (command[i])
            {
                case "ADD":
                    assembly += Add();
                    break;
                case "SUB":
                    assembly += Sub();
                    break;
                case "MUL":
                    assembly += Mul();
                    break;
                case "DIV":
                    assembly += Div();
                    break;
                case "MOV":
                    assembly += Mov();
                    break;
                case "VAR":
                    assembly += Var();
                    break;
                case "BRANCH":
                    assembly += Branch();
                    break;
                case "GOTO":
                    assembly += Goto();
                    break;
                case "PRINT":
                    assembly += Print();
                    break;
                case "INC":
                    assembly += Increment();
                    break;
                case "DEC":
                    assembly += Decrement();
                    break;
                case "FOR":
                    assembly += For();
                    break;
                case "IF":
                    assembly += If();
                    break;
                default:
                    assembly += command[i] + " ";
                    break;
            }
        }
    }

    public bool IsNumeric(string value)
    {
        return value.All(char.IsNumber);
    }


    string IsNumericVar(string operand1)
    {
        if (IsNumeric(operand1))
        {
            return "push " + operand1 + " ";
        }
        else
        {
            return "push " + operand1 + " load ";
        }
    }


    string math(string op, string operand1, string operand2)
    {
        string str = "";
        str += IsNumericVar(operand2);
        str += IsNumericVar(operand1);
        return str + op + " ";
    }

    string Add()
    {
        String operand1 = command[++i];
        String operand2 = command[++i];
        return math("add", operand1, operand2);
    }

    string Sub()
    {
        String operand1 = command[++i];
        String operand2 = command[++i];
        return math("sub", operand1, operand2);
    }

    string Mul()
    {
        String operand1 = command[++i];
        String operand2 = command[++i];
        return math("mul", operand1, operand2);
    }

    string Div()
    {
        String operand1 = command[++i];
        String operand2 = command[++i];
        return math("div", operand1, operand2);
    }

    string Mov()
    {
        String operand1 = command[++i];
        return String.Format("push {0} mov ", operand1);
    }

    string Var()
    {
        String operand1 = Mov();
        String operand2 = command[++i];
        return String.Format("push {0} {1}", operand2, operand1);
    }

    string Branch()
    {
        return String.Format("branch {0} ", command[++i]);
    }

    string Goto()
    {
        String _operand1 = command[++i];
        String _operator = command[++i];
        String _operand2 = command[++i];
        return math(_operator, _operand1, _operand2) + Branch();
    }

    string Print()
    {
        return String.Format("push {0} load print ", command[++i]);
    }

    string Increment()
    {
        string operand = command[++i];
        return math("add", operand, "1") + String.Format("push {0} mov ", operand);
    }
    
    string Decrement()
    {
        string operand = command[++i];
        return math("sub", operand, "1") + String.Format("push {0} mov ", operand);
    }

    string scope()
    {
        string com = "";
        if (command[++i] == "[")
        {
            string follow = command[++i];
            while (follow != "]")
            {
                com += follow  + " ";
                follow = command[++i];
            }
        }

        return com;
    }

    string For()
    {
        string var = command[++i];
        string deb = command[++i];
        string __ = command[++i];
        string fin = command[++i];
        
        string increment = "INC";
        string cond = "lt";
        if (int.Parse(deb) > int.Parse(fin))
        {
            increment = "DEC";
            cond = "gt";
        }

        string com = scope();

        string _for = String.Format("VAR {4} {0} for{6}: {2} {4} {3} {4} GOTO {4} {5} {1} for{6}", deb, fin, increment, com, var, cond, forN);
        forN++;
        return new Assembly(_for).assembly;
    }

    string If()
    {
        string _operand1 = command[++i];
        string _operator = command[++i];
        string _operand2 = command[++i];
        
        string _elseScope = scope();
        string _elseWord = command[++i];
        string _ifScope = scope();

        string _if = String.Format("GOTO {0} {2} {1} if{3} {4}push true BRANCH endif{3} if{3}: {5}endif{3}:",_operand1, _operand2, _operator, ifN, _elseScope, _ifScope);
        ifN++;
        return new Assembly(_if).assembly;
    }

    public string Parse()
    {
        return this.assembly + "halt";
    }

}
    
