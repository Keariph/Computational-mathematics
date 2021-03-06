// Three method.cpp: определяет точку входа для консольного приложения.
//

#include "stdafx.h"
#include <iostream>
#include <cmath>
using namespace std;
void HalfDivision(double a, double b, double E);
void Hord(double a, double b, double E);
void Newton(double a, double b, double E);
double FF(double x);
double FFF = 2;

double F(double x)
{
	double f = pow(x, 2) - 3;
	return f;
}
double FF(double x)
{
	double f = 2 * x;
	return f;
}
int main()
{
	double a, b;
	int enter;
	double E = 0.00000001;
	cout << "Enter a interval\nfrom ";
	cin >> a;
	cout << "to ";
	cin >> b;
	cout << "Select a method\n1.Half division\t2. Method hord\t 3. Newton's method\n";
	cin >> enter;
	switch (enter)
	{
	case 1:
		HalfDivision(a, b, E);
		break;
	case 2:
		Hord(a, b, E);
		break;
	case 3:
		Newton(a, b, E);
		break;
	default:
		cout << "Incorrect enter\n";
	}
    return 0;
}

void HalfDivision(double a, double b, double E)
{
	double c, fA, fC;
	int i = 1;
		while(abs((b - a)/2) > E)
		{
			c = (a + b) / 2;
			fA = F(a);
			fC = F(c);
			if ((fA * fC) < 0){ b = c;}
			else { a = c;}
			cout << "a = " << a << " b = " << b << endl;
			cout << "x"<<i<<" = " << c <<" +" <<E<<endl;
			i++;
		}
}

void Hord(double a, double b, double E)
{
	int  i = 1;
	double fA, fX, fB, x1 = 0, x2 = -1;
	while (fabs(x1 - x2) > E)
	{
		x2 = x1;
		fA = F(a);
		fB = F(b);
		fX = F(x1);
		x1 = ((a* fB) - (b*fA)) / (fB - fA);
		if ((fA * fX) < 0){ b = x1; }
		else { a = x1; }
		cout << "x" << i << " = " << x1 << " +" << E << endl;
		i++;
	}
	x1 = ((a* fB) - (b*fA)) / (fB - fA);
	cout << "x" << i << " = " << x1 << " +" << E << endl;
}

void Newton(double a, double b, double E)
{
	int n = 0;
	double x = 0, x1 = 0;
	int N = fabs(log((b - a) / 2) / log(2));
	cout << "N = " << n << endl;
	x = -(F(a) / FF(a)) + a;
	while (fabs(x - x1) > E)
	{
	x1 = x;
	x = x1 - F(x1) / FF(x1);
	cout << "x" << n << " = " << x1 << " +" << E << endl;
	n++;
	}
}
