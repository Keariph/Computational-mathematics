// Polynomial_interpolation.cpp: определяет точку входа для консольного приложения.
//

#include "stdafx.h"
#include <iostream>
#include <fstream>
#include <vector>
using namespace std;
double Langrange()
{
	double x = 2.1;
	char buf;
	//Создание и заполнение вектора
	int size = 3;
	vector<vector<double>> mas(2);
	for (int x = 0; x < size; x++)
	{
		mas[x].resize(size);
	}
	/*ifstream fin("mas.txt");
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size; j++)
		{
			fin >> buf;
			mas[i][j] = buf;
		}
	}*/
	//Нахождение L
	vector<double>L(size);
	L[0] = ((x - L[1])*(x - L[2])) / ((L[0] - L[1])*(L[0] - L[2]));
	L[1] = ((x - L[0])*(x - L[2])) / ((L[1] - L[0])*(L[1] - L[2]));
	L[2] = ((x - L[0])*(x - L[1])) / ((L[2] - L[0])*(L[2] - L[1]));
	for (int x = 0; x < size; x++)
	{
		cout << "L" << x << " = " << L[x] << endl;
	}
	return 0;
}
double Aitken()
{
	return 0;
}

double Newton1()
{
	return 0;
}

double Newton2()
{
	return 0;
}

int main()
{
	int a;
	while (true)
	{
		cout << "Select a method\n1. Langrange\t2. Aitken\t3. Newton 1\t4. Newton 2\t 5. exit\n";
		cin >> a;
		switch (a)
		{
		case 1: Langrange();
			break;
		case 2: Aitken();
			break;
		case 3: Newton1();
			break;
		case 4: Newton2();
			break;
		case 5: exit(0);
			break;
		default:
			break;
		}
	}
    return 0;
}

