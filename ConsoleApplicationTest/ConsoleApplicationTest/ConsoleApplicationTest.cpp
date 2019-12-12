// ConsoleApplicationTest.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//

#include "pch.h"
#include<iostream>
#include <string>
#include <list>
#include <sstream>
using namespace std;
list<string> split(string s, string del) {
	size_t pos = s.find(del);
	list<string> res;
	s = s + del;
	while (pos != s.npos) {
		res.push_back(s.substr(0, pos));
		s = s.substr(pos + del.length(), s.size());
		pos = s.find(del);
	}
	return res;
}
int main() {
	string in;
	stringstream stream;
	cin >> in;
	list<string> res = split(in, " ");
	int a, b;
	list<string>::iterator it = res.begin();
	string s = *it;
	stream << s;
	stream >> a;
	it++;
	s = *it;
	stream << s;
	stream >> b;
	int c = a - b;
	stream << c;
	stream >> s;
	int i = s.size();
	while (i != 0) {
		s.insert(i - 3, ",");
		i = i - 3;
	}
	cout << s;

}
