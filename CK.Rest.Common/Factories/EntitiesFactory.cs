using System.Collections.Generic;

using CK.Entities;
using CK.Rest.Common.Shared;

namespace CK.Rest.Common.Factories
{
    public static class EntitiesFactory
    {
        #region Public Methods

        public static IEnumerable<Language> GetDefaultLanguages()
        {
            return new Language[]
                    {
                        new Language(1, "csharp"),
                        new Language(2, "python"),
                        new Language(3, "javascript"),
                        new Language(4, "java"),
                    };
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1137:Elements should have the same indentation", Justification = "Must have this identation")]
        public static IEnumerable<Post> GetDefaultPosts()
        {
            return new Post[]
                    {
new Post(
1,
2,
"Reversing list python's style",
"A very cleaver and simple way reverse list in python usin the slicing operator",
2,
@"
fruits = ['Banana','Mango','Apple']
fruits_reverse = fruits[::-1]

print(fruits_reverse)

# ['Apple','Mango','Banana']
"),
new Post(
2,
2,
"The zip function, the not so common iterator's joiner",
"Sometimes we want to join two or more iterator into a single object, we can archieve that with zip!",
2,
@"
fruits = ['Banana','Mango','Apple']
ids = [1,2,3]

print zip(ids, fruits)

# [(1,'Banana'),(2,'Mango'),(3,'Apple')]
"),
new Post(
3,
3,
"A way to validate arguments",
"We can use a function as default to validate an argument in another function",
3,
@"
const isRequired = () => { throw new Error('argumen is required!'); };
const print = (value = isRequired()) => { console.log(`${value}`) };

print(4);     // logs 4
print(null);  // logs null
print();      // throws error
"),
new Post(
4,
3,
"Difference between the single and double quote with the + operator",
"It's important to take into account that the sum of chars return and integer, let's see it on the following example",
4,
@"
System.out.println(""A"" + ""B""); // AB
System.out.println('A' + 'B');     // 131
"),
new Post(
5,
4,
"Use yield when returning enummerable instad temporal objects",
"When we want to return a collection inside a function we usualy create a temporal enummberable , we filled and we return it, there is a better aproach thou",
1,
@"
public static IEnumerable<int> GetOdd(IEnumerable<int> numbers)
{
	var result = new List<int>();
	foreach(var number in numbers)
		if(number % 2 != 0)
			result.Add(number);
	return result;
}

public static IEnumerable<int> GetOddWithYield(IEnumerable<int> numbers)
{
	foreach(var number in numbers)
		if(number % 2 != 0)
			yield return number;
}
"),
new Post(
6,
4,
"Using enum as flags to allow combinations",
"The enums are a very powerfull way to strong type options but some times we want to use more than one, in those cases we can use them as flags",
1,
@"
[Flags]
enum Position
{
    Up = 2,
    Down = 4,
    Left = 8,
    Right = 16
}

Console.WriteLine((Position)10); // This will print 'Up, Left' (2 and 8)
"),
new Post(
7,
3,
"A quick way to format JSON code for presentation",
"Sometimes we want to present some JSON object to the user, in those cases we can format it with this function",
3,
@"
const prettyPrint = (obj) => JSON.stringify(obj,null,'\t')

const person = {id: 1, name:'Jane', Age:25}

console.log(prettyPrint(person));

// Output
/*
{
    'id': 1,
    'name': 'Jane',
    'Age': 25
}
*/
"),
new Post(
8,
3,
"Filtering unique values from an array",
"We can use the Set object to filter the repeated values from an array",
3,
@"
const repeated = [1, 2, 2, 3, 3, 3, 4, 4, 4, 4]
const unique = [...new Set(repeated)];

console.log(unique)
// [1, 2, 3, 4]
"),
new Post(
9,
2,
"This is one for those hackathons aficionado, 'factors of a number'",
"The 'factors of a numbers is often use as part of many programming test, this s a quick way in python to accomplish it'",
2,
@"
f = 30
for i in range(1, f + 1):
    if f % i == 0:
      print(i, end =' ')

# 1 2 3 5 6 10 15 30
"),
new Post(
10,
2,
"To '===' or not to '==', that is the question... for equality",
"One of the most common surprise comming from another language is this way to test equality, one test value, the ohter also type",
3,
@"
const num1 = '1'
const num2 =  1

console.log(num1 == num2);  // true, same value
console.log(num1 === num2); // false, same value but different types!
"),
new Post(
11,
3,
"Always prefer primitive types over wrapper classes",
"This maybe common sence but it also have it other motives to do it as we will see on the next snippet",
4,
@"
var num1 = 5;
var num2 = 5;

var num3 = new Integer(5);
var num4 = new Integer(5);

System.out.println(num1 == num2); // true
System.out.println(num3 == num4); // false
"),
new Post(
12,
3,
"Another reason to prefer primitives",
"In this oportunity we see the string primitive object, using the contructor is actually slower than the primitive",
4,
@"
var str1 = ""Hello!""              // Is a literal
var str2 = new String(""Hello!"")  // Is an object

var str3 = str1                    // Uses same 'pool' as str1, same memory
var str4 = ""Hello!""              // Also uses same 'pool' as str1, same memory

var str5 = new String(""Hello!"")  // Is an object with its own memory different from str2
"),
new Post(
13,
4,
"If and object implements IDisposable use the 'using' statement",
"Sometimes objects need to do some clean up no matter what for this we implement the IDisposable interface to call it on a finally block, but there is a better way",
1,
@"
// Old fashion
var myDisposable = new DisposableClass();
try
{
    // Do something with myDisposable that could throw
}
finally
{
    // Call disposable to do the clean up
    myDisposable.Dispose();
}

// Better way
using(var myDisposable = new DisposableClass())
{
    // It will always call Dispose() for us no matter what
}
"),
new Post(
14,
2,
"A cool trick to finding a position missing in a numeric array",
"This is a neat trick to quick finding the missing spot of a number in an array of numbers",
4,
@"
var array = new int[] { 1, 2, 3, 4, 6, 7, 8 };

int position = Arrays.binarySearch(array, 5);
System.out.print(~position); // notice the ~ operator!

// Prints 4!
"),
new Post(
15,
4,
"How to check the usage of memory of a variable?",
"Sometime you are bound to control the memory size of your variable, for example in transmisions of data, this is a way to measure",
2,
@"
import sys

a, b, c = 'hellow!' , 10, 59.99
print('a =', sys.getsizeof(a), 'bytes')
print('b =', sys.getsizeof(b), 'bytes')
print('c =', sys.getsizeof(c), 'bytes')

'''
a = 56 bytes
b = 28 bytes
c = 24 bytes
'''
"),
new Post(
16,
4,
"Quick swap two variables in one line",
"The way python asigment work allow for less code, to be precise you can do swap of variables in one statement",
2,
@"
x = 1
y = 2

x, y = y, x

print('x =', x)
print('y =', y)

'''
x = 2
y = 1
'''
"),
new Post(
17,
2,
"A neat way to initialize an object",
"For those who are use to old fashion objects initialization there is a neat way to do it on csharp",
1,
@"
class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

// Commom way
var person = new Person();
person.Name = ""John"";
person.Age = 35

// Cool way
var person = new Person
{
    Name = ""Jonh"",
    Age = 35
};
"),
new Post(
18,
2,
"Here is another amazing use for zip!",
"In python we can use zip with the * operator to transpose a matrix, pretty neat!",
2,
@"
x = [[11 ,22],
     [33 ,44],
     [55 ,66]]

x2 = zip(*x)

for row in x2:
    print(row)

'''
(11, 33, 55)
(22, 44, 66)
'''
"),
                    };
        }

        public static IEnumerable<User> GetDefaultUsers()
        {
            return new User[]
                    {
                        new User(1, "admin@ck.com", "admin".ToSha256(), "Sistem", "Admin", isAdmin: true),
                        new User(2, "jdoe@ck.com", "jdoe".ToSha256(), "John", "Doe"),
                        new User(3, "jpoe@ck.com", "jpoe".ToSha256(), "Jane", "Poe"),
                        new User(4, "rroe@ck.com", "rroe".ToSha256(), "Richard", "Roe"),
                    };
        }

        #endregion Public Methods
    }
}