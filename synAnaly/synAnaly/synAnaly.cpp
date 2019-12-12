// synAnaly.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//

#include "pch.h"
#include <iostream>
#include <string.h>
#include<istream>
#include<fstream>
using namespace std;

ifstream file;
ofstream ofile;               //定义输出文件

int syn = 0;
int constnum = 0, varnum = 0;		//常数个数，变量个数
const char* keyWord[5] = { "if","else","int", "float","while"};		//关键字
string varList[20];			//存储变量
int constList[20];			//存储常量
char token[10];		//存放识别的字符串

void scaner(char* lines, int length, int row)
{
	int m = 0;		//正在读第几个字符
	int n;		//token中第几个字符
	char ch = lines[m];		//当前字符
	int isVar;		//0 保留字   1 变量

	while (m < length) {			//扫描完这一行
		if (syn == 85) break;
		n = 0;
		for (int i = 0; i < 10; i++) token[i] = NULL;
		isVar = 1;
		while (ch == ' ') {
			m++;
			if (m < length) {
				ch = lines[m];
			}
		}

		if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z')) {		//关键字   if  1   else  2    int 3    float 4       或 变量名 90
			while ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9') || ch == '_') {
				token[n++] = ch;
				m++;
				if (m < length) {
					ch = lines[m];

				}
			}
			//与保留字/数据类型作比较
			for (int i = 0; i < 4; i++) {
				if (!strcmp(keyWord[i], token)) {
					isVar = 0;
					syn = 1 + i;
					cout << "<" << syn << "," << token << ">" << endl;
					ofile << "<" << syn << "," << token << ">" << endl;
					break;
				}
			}

			//若为变量
			if (isVar == 1) {
				syn = 90;
				int i;		//第几个变量，从0开始

				for (i = 0; i < varnum; i++) {			//是否已经存在
					if (!strcmp(varList[i].c_str(), token)) {
						break;
					}
				}

				if (i == varnum) {						//若未存在
					char* t;
					t = token;
					varList[i] = t;
					varnum++;
				}
				cout << "<" << syn << "," << i << ">" << endl;
				ofile << "<" << syn << "," << i << ">" << endl;
			}
		}

		else if ((ch >= '0' && ch <= '9')) {		//常量 91
			int sum = 0;		//记录常量值

			while ((ch >= '0' && ch <= '9')) {
				sum = sum * 10 + ch - '0';
				m++;
				if (m < length) {
					ch = lines[m];
				}
			}
			if (m < length && ch != ' ' && ch != '(' && ch != ')' && ch != '{' && ch != '}' && ch != ';' && ch != '#'
				&& ch != '+' && ch != '-' && ch != '*' && ch != '/' && ch != '>' && ch != '<'
				&& ch != '=' && ch != '!' && ch != '&' && ch != '|') {			//数字表示出错
				syn = 0;
				cout << "<" << syn << ", 数字表示出错 ERROR at line " << row << ">" << endl;
				ofile << "<" << syn << ", 数字表示出错 ERROR at line " << row << ">" << endl;
			}
			else {
				syn = 91;
				int i;
				for (i = 0; i < constnum; i++) {			//是否已经存在
					if (constList[i] == sum) {
						break;
					}
				}
				if (i == constnum) {
					constList[i] = sum;
					constnum++;
				}
				cout << "<" << syn << "," << i << ">" << endl;
				ofile << "<" << syn << "," << i << ">" << endl;
			}
		}

		else {				//其他字符
			switch (ch) {
			case'+': syn = 40; token[0] = ch; ch = lines[++m]; break;
			case'-': syn = 41; token[0] = ch; ch = lines[++m]; break;
			case'*': syn = 42; token[0] = ch; ch = lines[++m]; break;
			case'/': syn = 43; token[0] = ch; ch = lines[++m]; break;
			case'=':
				if (lines[m + 1] == '=') {
					syn = 60; token[0] = ch; token[1] = '='; m = m + 2; ch = lines[m];
				}
				else {
					syn = 44; token[0] = ch; ch = lines[++m];
				}
				break;
			case'!':
				if (lines[m + 1] == '=') {
					syn = 61; token[0] = ch; token[1] = '='; m = m + 2; ch = lines[m];
				}
				else {
					syn = 68; token[0] = ch; ch = lines[++m];
				}
				break;
			case'>':
				if (lines[m + 1] == '=') {
					syn = 64; token[0] = ch; token[1] = '='; m = m + 2; ch = lines[m];
				}
				else {
					syn = 62; token[0] = ch; ch = lines[++m];
				}
				break;
			case'<':
				if (lines[m + 1] == '=') {
					syn = 65; token[0] = ch; token[1] = '='; m = m + 2; ch = lines[m];
				}
				else {
					syn = 63; token[0] = ch; ch = lines[++m];
				}
				break;
			case'&':
				if (lines[m + 1] == '&') {
					syn = 66; token[0] = ch; token[1] = '&'; m = m + 2; ch = lines[m];
				}
				break;
			case'|':
				if (lines[m + 1] == '|') {
					syn = 67; token[0] = ch; token[1] = '|'; m = m + 2; ch = lines[m];
				}
				break;
			case',': syn = 79; token[0] = ch; ch = lines[++m]; break;
			case';': syn = 80; token[0] = ch; ch = lines[++m]; break;
			case'(': syn = 81; token[0] = ch; ch = lines[++m]; break;
			case')': syn = 82; token[0] = ch; ch = lines[++m]; break;
			case'{': syn = 83; token[0] = ch; ch = lines[++m]; break;
			case'}': syn = 84; token[0] = ch; ch = lines[++m]; break;
			case'#': syn = 85; token[0] = ch; ch = lines[++m]; break;
			default:syn = 0; m++; break;
			}

			if (token[0] == NULL) {
				cout << "<0,ERROR at line" << row << ">" << endl;
				ofile << "<0,ERROR at line" << row << ">" << endl;
			}
			else {
				cout << "<" << syn << "," << token << ">" << endl;
				ofile << "<" << syn << "," << token << ">" << endl;
			}
		}
	}
}

int main()
{
	char lines[40];		//保存每一行
	int length;		//每一行字符数
	int row = 0;		//第几行

	const char* filePath = "C:\\Users\\30544\\Desktop\\test1.txt";
	file.open(filePath, ios::in);
	if (!file.is_open())
		return 0;
	std::string strLine;

	ofile.open("C:\\Users\\30544\\Desktop\\result1.txt");     //作为输出文件打开


	while (getline(file, strLine))			//逐行读取
	{
		row++;
		if (strLine.empty())
			continue;
		for (int i = 0; i < strLine.length(); i++) {
			lines[i] = strLine.c_str()[i];
		}
		length = strLine.length();
		cout << strLine << endl;
		scaner(lines, length, row);
		if (syn == 85) break;
	}

	//变量表 varList
	ofile << endl << "变量：" << endl << "-----------------" << endl;
	for (int i = 0; i < varnum; i++) {
		ofile << i << "\t" << varList[i] << endl;
	}

	//常量表 constList
	ofile << endl << "常量：" << endl << "-----------------" << endl;
	for (int i = 0; i < constnum; i++) {
		ofile << i << "\t" << constList[i] << endl;
	}

}