# Boolean Algebra Expression Solver
This application is a simple boolean algebra expression solver coded in C# using WPF. The purpose of this program was to help me learn WPF as well as utilise reverse polish notation and the shunting yard algorithm. This application takes a boolean expression as an input and outputs its corresponding truth table.

## How To Use
1. Insert the number of terms you wish to evaluate in the "Number Of Terms" textbox
2. Insert your desired expression to be evaluated into the "Expression" textbox

### Symbols Used
The symbols used in this program include:
* \* as __AND__
* \+ as __OR__
* ^ as __XOR__
* ' as __NOT__
* () as order of operations and for grouping

### Examples Of Expression
A example of an expression would be:
(2 + 1') ^ 3 * 4

## Prerequisites
This program requires no prerequisites

## Built With
This program was built with no external frameworks excluding the C# system libraries, all additional libraries included, such as the maths class was created by me, Shaan Khan.

## Authors
* **Shaan Khan** - *All Work*

## License
This project is licensed under the Mozilla Public License 2.0 - see the [LICENSE](https://github.com/ShaanCoding/Boolean-Expression-Solver/blob/master/LICENSE) files for details
