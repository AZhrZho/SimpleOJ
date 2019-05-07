# SimpleOJ接口文档
---
## 注册账号
- url：http://localhost:8081/?app=register
- post：json
- 参数:
  - sno:账号
  - pw:密码
- 返回参数:
  - Code:http状态码
  - Message:文字消息
  - Data：返回数据(bool),指示注册是否成功
- 测试样例:
``` json
{
	"sno": "lizhuoqiang",
	"pw": "12345"
}
```
``` json
{
    "Code": 200,
    "Message": "注册成功",
    "Data": true
}
```

## 登录
- url：http://localhost:8081/?app=login
- post：json
- 参数:
  - sno:账号
  - pw:密码
- 返回参数:
  - Code:http状态码
  - Message:文字消息
  - Data：返回数据(string),值为sessionID
- 测试样例:
``` json
{
	"sno": "lizhuoqiang",
	"pw": "12345"
}
```
``` json
{
    "Code": 200,
    "Message": "登陆成功",
    "Data": "4832df8b-ad62-4b92-912b-de71eba460a2"
}
```
> 注：sessionID的有效期为300分钟，过期后需重新登陆

## 获取题目记录
- url：http://localhost:8081/?app=list
- post：none
- 返回参数:
  - Code:http状态码
  - Message:文字消息
  - Data：返回数据(array),所有题目的信息
- 测试样例:
``` json
{
    "Code": 200,
    "Message": "获取题目信息成功",
    "Data": [
        {
            "ID": 1,
            "Name": "测试题",
            "TimeLimit": 1000,
            "Discription": "输入包含三个测试数据，每个测试数据都是整数。要求原样输出这些数据",
            "SampleInput": "1\r\n2\r\n3\r\n",
            "SampleOutput": "1\r\n2\r\n3\r\n"
        },
        {
            "ID": 2,
            "Name": "两数相加",
            "TimeLimit": 1000,
            "Discription": "输出两数之和",
            "SampleInput": "1 2\r\n",
            "SampleOutput": "3\r\n"
        },
        {
            "ID": 3,
            "Name": "两数相乘",
            "TimeLimit": 1000,
            "Discription": "返回两数之积",
            "SampleInput": "4 5\r\n",
            "SampleOutput": "20\r\n"
        }
    ]
}
```

## 获取提交列表
- url：http://localhost:8081/?app=posts
- post：none
- 返回参数:
  - Code:http状态码
  - Message:文字消息
  - Data：返回数据(array),所有提交记录
- 测试样例:
``` json
{
    "Code": 200,
    "Message": "获取提交记录成功",
    "Data": [
        {
            "id": 67,
            "problem": 1,
            "language": "c",
            "runtime": 67,
            "result": "WA",
            "sno": "lizhuoqiang",
            "time": "2019-05-07T14:42:39"
        },
        {
            "id": 68,
            "problem": 1,
            "language": "c",
            "runtime": 94,
            "result": "WA",
            "sno": "lizhuoqiang",
            "time": "2019-05-07T14:56:49"
        },
        {
            "id": 69,
            "problem": 1,
            "language": "c",
            "runtime": 65,
            "result": "WA",
            "sno": "lizhuoqiang",
            "time": "2019-05-07T14:56:56"
        },
        {
            "id": 70,
            "problem": 1,
            "language": "c",
            "runtime": 60,
            "result": "WA",
            "sno": "lizhuoqiang",
            "time": "2019-05-07T14:56:59"
        },
        {
            "id": 71,
            "problem": 1,
            "language": "c",
            "runtime": 56,
            "result": "WA",
            "sno": "lizhuoqiang",
            "time": "2019-05-07T14:57:01"
        },
        {
            "id": 72,
            "problem": 1,
            "language": "c",
            "runtime": 56,
            "result": "WA",
            "sno": "lizhuoqiang",
            "time": "2019-05-07T14:57:02"
        }
    ]
}
```

## 提交代码
- url：http://localhost:8081/?app=judge&id=1&lan=c
- 参数:
  - id:题目编号
  - lan:使用的语言
    >可能的值：c、c++、java(java尚不支持)
- post：json
- 参数:
  - code:代码
  - session:用户的sessionID
- 返回参数:
  - Code:http状态码
  - Message:文字消息
  - Data：返回数据(array),表示题目状态
  - Data.Result值：
    - 0 - AC    
    - 1 - WA    
    - 2- EA
- 测试样例1:
``` json
{
    "code":"
        #include<stdio.h>
	    int main(){
	    int a,b;
	    printf(\"%d\\n\",b);
	    return 0;
    }",
    "session":"c0aedce5-e867-40d0-acef-18c4c0095709"
}
```
``` json
{
    "Code": 200,
    "Message": "答案错误",
    "Data": {
        "Time": 61,
        "Pass": false,
        "Result": 1
    }
}
```
- 测试样例2:
``` json
{
	"code":"some code",
	"session":"12345"
}
```
``` json
{
    "Code": 200,
    "Message": "session验证失败",
    "Data": null
}
```
