@[TOC](Sevice Computing：CLI 命令行实用程序开发基础)

# 1、概述
CLI（Command Line Interface）实用程序是Linux下应用开发的基础。正确的编写命令行程序让应用与操作系统融为一体，通过shell或script使得应用获得最大的灵活性与开发效率。Linux提供了cat、ls、copy等命令与操作系统交互；go语言提供一组实用程序完成从编码、编译、库管理、产品发布全过程支持；容器服务如docker、k8s提供了大量实用程序支撑云服务的开发、部署、监控、访问等管理任务；git、npm等都是大家比较熟悉的工具。尽管操作系统与应用系统服务可视化、图形化，但在开发领域，CLI在编程、调试、运维、管理中提供了图形化程序不可替代的灵活性与效率。

# 2、基础知识
命令行是程序与用户进行交互的一种手段，具有可靠的复杂命令行参数处理机制，会使得应用程序更好、更有用。

通过阅读老师给的两篇参考资料，基本理解了POSIX/GNU 命令行接口的一些概念与规范。命令行程序主要涉及内容：
>- 命令
>- 命令行参数
>- 选项：长格式、短格式
>-  IO：stdin、stdout、stderr、管道、重定向
>- 环境变量

## 命令
- 命令行准则：

通用 Linux 实用程序的编写者应该在代码中遵守某些准则。这些准则经过了长期发展，它们有助于确保用户以更灵活的方式使用实用程序，特别是在与其它命令（内置的或用户编写的）以及 shell 的协作方面 ― 这种协作是利用 Linux 作为开发环境的能力的手段之一。selpg 实用程序用实例说明了下面列出的所有准则和特性。（注：在接下来的那些示例中，“$”符号代表 shell 提示符，不必输入它。）

**准则1:输入**
应该允许输入来自以下两种方式：
在命令行上指定的文件名。例如：
`$ command input_file`
在这个例子中，command 应该读取文件 input_file。
标准输入（stdin），缺省情况下为终端（也就是用户的键盘）。例如：
`$ command `
这里，用户输入 Control-D（文件结束指示符）前输入的所有内容都成为 command 的输入。

**准则2:输出**
输出应该被写至标准输出，缺省情况下标准输出同样也是终端（也就是用户的屏幕）：
`$ command `
在这个例子中，command 的输出出现在屏幕上。

**准则 3. 错误输出**
错误输出应该被写至标准错误（stderr），缺省情况下标准错误同样也是终端（也就是用户的屏幕）：
`$ command `
这里，运行 command 时出现的任何错误消息都将被写至屏幕。
但是使用标准错误重定向，也可以将错误重定向至文件。例如：
`$ command 2>error_file`
在这个例子中，command 的正常输出在屏幕显示，而任何错误消息都被写至 error_file。
可以将标准输出和标准错误都重定向至不同的文件，如下所示：
`$ command >output_file 2>error_file`
这里，将标准输出写至 output_file，而将所有写至标准错误的内容都写至 error_file。

**准则 4. 执行**
程序应该有可能既独立运行，也可以作为管道的一部分运行，如上面的示例所示。该特性可以重新叙述如下：不管程序的输入源（文件、管道或终端）和输出目的地是什么，程序都应该以同样的方式工作。这使得在如何使用它方面有最大的灵活性。

**准则 4. 执行**
程序应该有可能既独立运行，也可以作为管道的一部分运行，如上面的示例所示。该特性可以重新叙述如下：不管程序的输入源（文件、管道或终端）和输出目的地是什么，程序都应该以同样的方式工作。这使得在如何使用它方面有最大的灵活性。
## 命令行参数
- 命令行参数的定义是：
> 命令行上除了命令名之外的字符串。参数由多项构成，项与项之间用空白符彼此隔开。参数进一步分为选项和操作数。选项用于修改程序的默认行为或为程序提供信息，比较老的约定是以短划线开头。选项后可以跟随一些参数称为选项参数，剩下的是操作数。

**准则 5. 命令行参数**
如果程序可以根据其输入或用户的首选参数有不同的行为，则应将它编写为接受名为 选项的命令行参数，这些参数允许用户指定什么行为将用于这个调用。
作为选项的命令行参数由前缀“-”（连字符）标识。另一类参数是那些不是选项的参数，也就是说，它们并不真正更改程序的行为，而更象是数据名称。通常，这类参数代表程序要处理的文件名，但也并非一定如此；参数也可以代表其它东西，如打印目的地或作业标识（有关的示例，请参阅“man cancel”）。
可能代表文件名或其它任何东西的非选项参数（那些没有连字符作为前缀的）如果出现的话，应该在命令的最后出现。
通常，如果指定了文件名参数，则程序把它作为输入。否则程序从标准输入进行读取。
所有选项都应以“-”（连字符）开头。选项可以附加参数。

Linux 实用程序语法图看起来如下：
`$ command mandatory_opts [ optional_opts ] [ other_args ]`
其中：
- command 是命令本身的名称。
- mandatory_opts 是为使命令正常工作必须出现的选项列表。
- optional_opts 是可指定也可不指定的选项列表，这由用户来选择；但是，其中一些参数可能是互斥的，如同 selpg 的“-f”和“-l”选项的情况（详情见下文）。
- other_args 是命令要处理的其它参数的列表；这可以是任何东西，而不仅仅是文件名。

在以上定义中，术语“选项列表”是指由空格、跳格或二者的结合所分隔的一系列选项。
以上在方括号中显示的语法部分可以省去（在此情况下，必须将括号也省去）。
各个选项看起来可能与下面相似：
>-f （单个选项）
-s20 （带附加参数的选项）
-e30 （带附加参数的选项）
-l66 （带附加参数的选项）

有些实用程序对带参数的选项采取略微不同的格式，其中参数与选项由空格分隔 ― 例如，“-s 20” ― 但我没有选择这么做，因为它会使编码复杂化；这样做的唯一好处是使命令易读一些。
以上是 selpg 支持的实际选项。

## 选项：长格式、短格式
短格式使用“ -”符号（半角减号符）引导开始选项，一般是单个英文字母，字母可以是大写也可以是小写。如
```javascript
$ ls -al
```
用到两个参数-a -l，所以还可以写成这样
```javascript
$ ls -a -l
```
长格式选项前用“--”（两个半角减号符）引导开始的，命令选项一般使用英文单词表示。一般不能组合使用。
##  IO：stdin、stdout、stderr、管道、重定向
- stdin、stdout、stderr
在通常情况下，UNIX每个程序在开始运行的时刻，都会有3个已经打开的stream： stdin, stdout, stderr - 标准 I/O 流。分别用来输入，输出，打印诊断和错误信息。
这3个symbols都是stdio(3) macro，类型为指向FILE的指针。可以被fprintf() fread（）等函数使用。
Linux的本质就是所有都是文件，输入输出设备也是以文件形式存在和管理的。
	> 内核启动的时候默认打开这三个I/O设备文件：标准输入文件stdin，标准输出文件stdout，标准错误输出文件stderr，分别得到文件描述符 0, 1, 2。 

- 重定向
	
	默认情况下始终有3个"文件"处于打开状态,stdin(键盘),stdout(屏幕),和stderr(错误消息输出到屏幕上).这3个文件和其他打开的文件都可以被重定向。

	 对于重定向简单的解释就是捕捉一个文件,命令, 程序,脚本, 或者是脚本中的代码块(请参考例子3-1和例子3-2)的输出,然后将这些输出作为输入发送到另一个文件,命令, 程序,或脚本中。

	 我们也可以用<符号来改变标准输入。

	每个打开的文件都会被分配一个文件描述符.[1] stdin, stdout,和stderr的文件描述符分别是0, 1, 和 2. 除了这3个文件,对于其他那些需要打开的文件,保留了文件描述符3到9.在某些情况下,将这些额外的文件描述符分配给stdin,stdout,或stderr作为临时的副本链接是非常有用的.[2] 在经过复杂的重定向和刷新之后需要把它们恢复成正常状态。
- 管道
	
	管道可以将一个命令的输出导向另一个命令的输入，从而让两个(或者更多命令)像流水线一样连续工作，不断地处理文本流。
	
	在命令行中，我们用|表示管道。
##  环境变量
关于环境变量，这个我们已经在之前的安装部分详细讲过，这里就不再赘述了。
[关于安装和环境变量配置的传送门](https://blog.csdn.net/weixin_40377691/article/details/100853336#2_64)
##  Golang之使用Flag和Pflag
**Package flag & pflag**
> 作用： Package flag implements command-line flag parsing. 
Go语言通过使用标准库里的flag包来处理命令行参数。
这里唯一指的注意的就是返回值：是指针。

pflag 包与 flag 包的工作原理甚至是代码实现都是类似的，下面是 pflag 相对 flag 的一些优势：
>* 支持更加精细的参数类型：例如，flag 只支持 uint 和 uint64，而 pflag 额外支持 uint8、uint16、int32 等类型。 
>* 支持更多参数类型：ip、ip mask、ip net、count、以及所有类型的 slice 类型。 
>* 兼容标准 flag 库的 Flag 和 FlagSet：pflag 更像是对 flag 的扩展。 
>* 原生支持更丰富的功能：支持 shorthand、deprecated、hidden 等高级功能。 

**常用函数**
- flag.String(), flag.Bool(), flag.Int(), flag.Float64() 返回对应类型的指针：
 `func Xxx(name string, value Xxx, usage string) *Xxx`
- flag.XxxVar() 将参数绑定到对应类型的指针：
`func XxxVar(p *Xxx, name string, value Xxx, usage string)`
- flag.Var() 绑定自定义类型： 
 `func Var(value Value, name string, usage string`
 自定义类型需要实现Value接口。Var定义了一个有指定名字和用法说明的标签。标签的类型和值是由第一个参数指定的，这个参数是Value类型，并且是用户自定义的实现了Value接口的类型
- flag.Parse() 解析命令行参数到定义的flag解析函数将会在碰到第一个非flag命令行参数时停止，非flag命令行参数是指不满足命令行语法的参数，如命令行参数为./selpg -s1 -e2 input.txt 则第一个非flag命令行参数为“input.txt”。
- flag.Args()，flag.Arg(i)，flag.NArg()
在命令行标签被解析之后（遇到第一个非flag参数），flag.NArg()就返回解析后参数的个数。
- flag.Usage() 输出命令行的提示信息
# 3、开发实践
使用 golang 开发 开发 Linux 命令行实用程序 中的 selpg
提示：
- 请按文档 使用 selpg 章节要求测试你的程序
- 请使用 pflag 替代 goflag 以满足 Unix 命令行规范， 参考：[Golang之使用Flag和Pflag](https://o-my-chenjian.com/2017/09/20/Using-Flag-And-Pflag-With-Golang/)
- golang 文件读写、读环境变量，请自己查 os 包
- “-dXXX” 实现，请自己查 os/exec 库，例如案例 Command，管理子进程的标准输入和输出通常使用 io.Pipe，具体案例见 Pipe

## slepg程序逻辑及源代码理解
- selpg 是从文本输入选择页范围的实用程序。该输入可以来自作为最后一个命令行参数指定的文件，在没有给出文件名参数时也可以来自标准输入。
- selpg 首先处理所有的命令行参数。在扫描了所有的选项参数（也就是那些以连字符为前缀的参数）后，如果 selpg 发现还有一个参数，则它会接受该参数为输入文件的名称并尝试打开它以进行读取。如果没有其它参数，则 selpg 假定输入来自标准输入。 

**参数处理**
- 强制选项：“-sNumber”和“-eNumber”
selpg 要求用户用两个命令行参数“-sNumber”（例如，“-s10”表示从第 10 页开始）和“-eNumber”（例如，“-e20”表示在第 20 页结束）指定要抽取的页面范围的起始页和结束页。
	```javascript
	$ selpg -s10 -e20 ...
	```
- 可选选项：“-lNumber”和“-f”
selpg 可以处理两种输入文本：
	- 文本的页行数固定。这是缺省类型，如果既没有给出“-lNumber”也没有给出“-f”选项，则 selpg 会理解为页有固定的长度（在我的程序中默认每页 20 行）。
该缺省值可以用“-lNumber”选项覆盖，如下所示：
	```javascript
	$ selpg -s10 -e20 -l66 ...
	```
	- 该类型文本的页由 ASCII 换页字符（十进制数值为 12，在 C 中用“\f”表示）定界。在含有文本的行后面，只需要一个字符 ― 换页 ― 就可以表示该页的结束。
	```javascript
	$ selpg -s10 -e20 -f ...
	```
	注：“-lNumber”和“-f”选项是互斥的。
- 可选选项：“-dDestination”
selpg 还允许用户使用“-dDestination”选项将选定的页直接发送至打印机。
	```javascript
	$ selpg -s10 -e20 -dlp1
	```
## 代码实现
首先安装spf13/pflag 包，以调用flag和pflag：
```javascript
$ go get github.com/spf13/pflag
```
设置程序的参数结构体。提取参数将值赋值给该结构体：
```javascript
type  selpg_args struct {
	start_page int  //开始页
	end_page int //结束页
	in_filename string  // 输入文件名 
	print_dest string	//输出文件名 
	page_len int  // 每页的行数
	page_type string  // 'l'按行打印，'f'按换页符打印，默认按行
}
```
**main函数**
和原函数基本一致，由于我们使用pflag绑定了sa的各个变量，可以省略掉一些初始化。
```javascript
func main() {
sa := sp_args{}
	progname = os.Args[0]
// 处理参数 
process_args(&sa)
// 处理输入输出 
process_input(sa)
}
```
**func process_args函数**
在这个函数中首先对命令行的参数进行参数值的绑定,通过 pflag.Parse()方法让pflag 对标识和参数进行解析。之后就可以直接使用绑定的值。
使用os.Args读取程序输入的所有参数，并进行合法性检验，包括对每个参数的格式是否正确进行判断，对参数的个数是否正确进行判断，还有参数大小是否在合法范围内进行判断等等。得到的值是包含参数的string数组，然后将参数的值提取出来赋值给结构体。
```javascript
func process_args(sa * sp_args) {
//将flag绑定到sa的各个变量上 
	flag.IntVarP(&sa.start_page,"start",  "s", -1, "start page(>1)")
	flag.IntVarP(&sa.end_page,"end", "e",  -1, "end page(>=start_page)")
	flag.IntVarP(&sa.page_len,"len", "l", 72, "page len")
	flag.StringVarP(&sa.print_dest,"dest", "d", "", "print dest")
	flag.StringVarP(&sa.page_type,"type", "f", "l", "'l' for lines-delimited, 'f' for form-feed-delimited. default is 'l'")
	flag.Lookup("type").NoOptDefVal = "f"
	flag.Usage = func() {
		fmt.Fprintf(os.Stderr,
"USAGE: \n%s -s start_page -e end_page [ -f | -l lines_per_page ]" + 
" [ -d dest ] [ in_filename ]\n", progname)
		flag.PrintDefaults()
	}
	flag.Parse()

// os.Args是一个储存了所有参数的string数组，我们可以使用下标来访问参数 
// if参数个数不够
if len(os.Args) < 3 {	
		fmt.Fprintf(os.Stderr, "\n%s: not enough arguments\n", progname)
		flag.Usage()
		os.Exit(1)
	}
// 处理第一个参数 - start page 
// if第一个参数不为's'或数值不在合法范围内
if os.Args[1] != "-s" {
		fmt.Fprintf(os.Stderr, "\n%s: 1st arg should be -s start_page\n", progname)
		flag.Usage()
		os.Exit(2)
	}
INT_MAX := 1 << 32 - 1
if(sa.start_page < 1 || sa.start_page > INT_MAX) {
		fmt.Fprintf(os.Stderr, "\n%s: invalid start page %s\n", progname, os.Args[2])
		flag.Usage()
		os.Exit(3)
	}
// 处理第二个参数 - end page 
//if第二个参数不为'e'
if os.Args[3] != "-e" {
		fmt.Fprintf(os.Stderr, "\n%s: 2nd arg should be -e end_page\n", progname)
		flag.Usage()
		os.Exit(4)
	}
//if end_page 数值不在合法范围内，且小于等于start_page
if sa.end_page < 1 || sa.end_page > INT_MAX || sa.end_page < sa.start_page {
		fmt.Fprintf(os.Stderr, "\n%s: invalid end page %s\n", progname, sa.end_page)
		flag.Usage()
		os.Exit(5)
	}
// 处理page_len 
if ( sa.page_len < 1 || sa.page_len > (INT_MAX - 1) ) {
		fmt.Fprintf(os.Stderr, "\n%s: invalid page length %s\n", progname, sa.page_len)
		flag.Usage()
		os.Exit(5)
	}
// 设置in_filename  
//检查是否还有剩余的参数。对于 selpg，最多有一个这样的参数，它被用作输入的文件名。
if len(flag.Args()) == 1 {
_, err := os.Stat(flag.Args()[0])
// 检查文件是否存在 
if err != nil && os.IsNotExist(err) {
			fmt.Fprintf(os.Stderr, "\n%s: input file \"%s\" does not exist\n",
					progname, flag.Args()[0]);
			os.Exit(6);
		}
		sa.in_filename = flag.Args()[0]
	}
/* page_len */ 
}

```
**func process_input函数**
和原函数类似的，我们先选择从哪里读取和在哪儿打印，接着按照page_type进行打印。当用户指定了输出地点时，我们通过cmd创建子程序“cat”， 帮助我们将输出流的内容打印到指定地点。
```javascript
func process_input(sa sp_args) {
// 输入流 
var fin *os.File 
// 设置输入流。输入可以来自终端（用户键盘），文件或另一个程序的输出
if len(sa.in_filename) == 0 {
		fin = os.Stdin
	} else {
var err error
		fin, err = os.Open(sa.in_filename)
if err != nil {
			fmt.Fprintf(os.Stderr, "\n%s: could not open input file \"%s\"\n",
				progname, sa.in_filename)
			os.Exit(7)
		}
defer fin.Close()
	}
//使用 bufio.NewReader 来获得一个读取器变量
bufFin := bufio.NewReader(fin)
// 设置输出地点。输出可以是屏幕，文件或另一个文件的输入 
var fout io.WriteCloser
cmd := &exec.Cmd{}
if len(sa.print_dest) == 0 {
		fout = os.Stdout
	} else {
		cmd = exec.Command("cat")
//用只写的方式打开 print_dest 文件，如果文件不存在，就创建该文件。 
var err error
		cmd.Stdout, err = os.OpenFile(sa.print_dest, os.O_APPEND|os.O_WRONLY, os.ModeAppend)
if err != nil {
			fmt.Fprintf(os.Stderr, "\n%s: could not open file %s\n",
				progname, sa.print_dest)
			os.Exit(8)
		}
//StdinPipe返回一个连接到command标准输入的管道pipe 
		fout, err = cmd.StdinPipe()
if err != nil {
			fmt.Fprintf(os.Stderr, "\n%s: could not open pipe to \"lp -d%s\"\n",
				progname, sa.print_dest)
			os.Exit(8)
		}
		cmd.Start()
defer fout.Close()
	}
//根据page_type（按固定行数或分页符进行打印） 
//当前页数 
var page_ctr int
if sa.page_type == "l" { //按固定行数打印 
line_ctr := 0
		page_ctr = 1
for {
//上文写到的bufFin := bufio.NewReader(fin)
line,  crc := bufFin.ReadString('\n')
if crc != nil {
break // 碰到eof 
			}
			line_ctr++
if line_ctr > sa.page_len {
				page_ctr++
				line_ctr = 1
			}
//到达指定页码，开始打印 
if (page_ctr >= sa.start_page) && (page_ctr <= sa.end_page) {
_, err := fout.Write([]byte(line))
if err != nil {
					fmt.Println(err)
					os.Exit(9)
				}
		 	}
		}  
	} else {			//按分页符打印 
		page_ctr = 1
for {
page, err := bufFin.ReadString('\n')
//txt 没有换页符，使用\n代替，而且便于测试
//line, crc := bufFin.ReadString('\f')
if err != nil {
break // eof
			}
//到达指定页码，开始打印
if (page_ctr >= sa.start_page) && (page_ctr <= sa.end_page) {
_, err := fout.Write([]byte(page))
if err != nil {
					os.Exit(5)
				}
			}
//每碰到一个换页符都增加一页 
			page_ctr++
		}
	}
//if err := cmd.Wait(); err != nil {
//handle err
if page_ctr < sa.start_page {
			fmt.Fprintf(os.Stderr,
"\n%s: start_page (%d) greater than total pages (%d)," +
" no output written\n", progname, sa.start_page, page_ctr)
		} else if page_ctr < sa.end_page {
			fmt.Fprintf(os.Stderr,"\n%s: end_page (%d) greater than total pages (%d)," +
" less output than expected\n", progname, sa.end_page, page_ctr)
		} 
}
```
## 测试运行
命令行格式如下：
```javascript
$ selpg -s startPage -e endPage [-l linePerPage | -f ][-d dest] filename
```
> 其中，-s表示开始打印的页码，-e表示结束打印的页码，这两个必须写上； 而-l表示按固定行数打印文件，-f表示按照换页符来打印，默认按行；-d则是打印的目的地，默认为屏幕。

按照老师的要求，我们在这里使用[开发 Linux 命令行实用程序](https://www.ibm.com/developerworks/cn/linux/shell/clutil/index.html)上面的测试用例
```javascript
$ selpg -s 1 -e 1 test.txt
```
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191007090858905.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MDM3NzY5MQ==,size_16,color_FFFFFF,t_70)
该命令将把“input_file”的第 1 页写至标准输出（也就是屏幕），因为这里没有重定向或管道。
在代码中我设定的默认一页有10行，所以这里输出到line10。
```javascript
$ selpg -s 1 -e 2 <test.txt
```
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191007090913130.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MDM3NzY5MQ==,size_16,color_FFFFFF,t_70)
该命令与示例 1 所做的工作相同，但在本例中，selpg 读取标准输入，而标准输入已被 shell／内核重定向为来自“input_file”而不是显式命名的文件名参数。输入的前2 页被写至屏幕。
```javascript
$ cat test.txt | selpg -s 2 -e 2
```
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191007090941712.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MDM3NzY5MQ==,size_16,color_FFFFFF,t_70)
“other_command”的标准输出被 shell／内核重定向至 selpg 的标准输入。将第 2 页到第 2 页写至 selpg 的标准输出（屏幕）。
```javascript
$ selpg -s 1 -e 2 test.txt >output.txt
```
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191007091002538.png)
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191007091047322.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MDM3NzY5MQ==,size_16,color_FFFFFF,t_70)
selpg 将第 1 页到第 2 页写至标准输出；标准输出被 shell／内核重定向至“output_file”。
```javascript
$ selpg -s 0 -e 2 test.txt >error.txt
```
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191007091100475.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MDM3NzY5MQ==,size_16,color_FFFFFF,t_70)
selpg 将第 0 页到第 2 页写至标准输出（屏幕）；所有的错误消息被 shell／内核重定向至“error_file”。
这里因为第0页是一个不合法的参数，所以会报错，并且显示help
```javascript
$ selpg -s 1 -e 2 test.txt 2>error.txt
```
![在这里插入图片描述](https://img-blog.csdnimg.cn/2019100709113570.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MDM3NzY5MQ==,size_16,color_FFFFFF,t_70)
和上一条类似，请注意：在“2”和“>”之间不能有空格；这是 shell 语法的一部分（请参阅“man bash”或“man sh”）。
```javascript
$ selpg -s 1 -e 2 test.txt >output.txt 2>error.txt
$ selpg -s 1 -e 2 test.txt >output.txt 2>/dev/null
$ selpg -s 1 -e 1 test.txt >/dev/null
```
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191007091221811.png)
- 第一条语句，selpg 将第 10页到第 20页写至标准输出，标准输出被重定向至“output_file”；selpg 写至标准错误的所有内容都被重定向至“error_file”。当“input_file”很大时可使用这种调用；这种方法可对输出和错误都进行保存。
- 第二条语句，selpg 将第 10页到第 2 页写至标准输出，标准输出被重定向至“output_file”；selpg 写至标准错误的所有内容都被重定向至 /dev/null（空设备），这意味着错误消息被丢弃了。设备文件 /dev/null 废弃所有写至它的输出，当从该设备文件读取时，会立即返回 EOF。即不保存错误。
- 第三条语句，selpg 将第 1 页到第 1 页写至标准输出，标准输出被丢弃；错误消息在屏幕出现。这可作为测试 selpg 的用途，此时您也许只想（对一些测试情况）检查错误消息，而不想看到正常输出。
```javascript
$ selpg -s 1 -e 1 test.txt | wc
$ selpg -s 1 -e 1 test.txt 2>error.txt | wc
```
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191007091332558.png)
- 第一条语句，selpg 的标准输出透明地被 shell／内核重定向，成为“other_command”的标准输入，第 1 页到第 2 页被写至该标准输入。“other_command”的示例是 wc，它会显示选定范围的页中包含的行数、字数和字符数。错误消息仍在屏幕显示。
- 与上一条语句相似，只有一点不同：错误消息被写至“error_file”。
```javascript
$ selpg -s 1 -e 1 -l 1 test.txt
```
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191007091436456.png)
该命令将页长设置为 1 行，这样 selpg 就可以把输入当作被定界为该长度的页那样处理。第 101页到第 1页被写至 selpg 的标准输出（屏幕）。可以看到本来一页是十个line，但是设置完-l参数以后，一页变成只有一行。
```javascript
$ selpg -s 1 -e 1 -f test.txt
```
![在这里插入图片描述](https://img-blog.csdnimg.cn/2019100709152627.png)
假定页由换页符定界。第 10页到第 1页被写至 selpg 的标准输出（屏幕）。

```javascript
$ selpg -s 1 -e 1 test.txt > output.txt 2>error.txt &
```
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191007091610518.png)
该命令利用了 Linux 的一个强大特性，即：在“后台”运行进程的能力。在这个例子中发生的情况是：“进程标识”（pid）如 1234 将被显示，然后 shell 提示符几乎立刻会出现，使得您能向 shell 输入更多命令。同时，selpg 进程在后台运行，并且标准输出和标准错误都被重定向至文件。这样做的好处是您可以在 selpg 运行时继续做其它工作。

[Github代码传送门](https://github.com/ZhengweiZhao/selpg)

