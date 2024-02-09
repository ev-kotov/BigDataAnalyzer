## BigDataAnalyzer
Пример применения многопоточности, с целью считывания и анализа данных, полученных из больших текстовых файлов (примерно 285 миллионов строк).

Файлы для тестирования можно скачать [отсюда](https://www.kaggle.com/datasets/mkechinov/ecommerce-behavior-data-from-multi-category-store).

## Запуск приложения
Приложение может быть запущено из командной строки Windows, со следующими обязательными аргументами:

`<1>`- Размер пакета (количество строк) направляемое на обработку

`<2>` - Абсолюный путь к текстовому файлу (можно указать несколько файлов, результаты будут объединены)

Общий вид:  `BigDataAnalyzer.exe` `<1>` `<2>`


## Пример
 ```cmd
BigDataAnalyzer.exe 500000 C:\Users\Dowloads\2019-Oct.csv C:\Users\Dowloads\2019-Nov.csv
```

## Вид
![Far_ZJR6RMbynL](https://github.com/ev-kotov/BigDataAnalyzer/assets/48629337/d9a89ead-21ce-4e4e-ac0f-744e0c41d8ed)

