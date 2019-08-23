## Исследование визуального языка программирования

### Цель

Придумать язык, который позволит комфортно создавать программы в трёхмерном пространстве с помощью VR. Язык позволит красиво иллюстрировать алгоритмы. Красотой кода смогут насладиться не только программисты. Такой код можно будет тематически оформлять. Скорей всего это будет дорогое удовольствие для привлечения программистов. Это скрасит разработку и даст возможность делать красивые презентации кода.

### Основные принципы
1. Красота
2. Компактность
3. Минимум контекста
4. Наглядность
5. Эргономика

## Прогресс
<table>
  <tr>
    <th width="50%">сейчас</th><th width="50%">цель</th>
  </tr>
  <tr>
    <td><img src="./now.svg?sanitize=true"/></td><td><img src="./target.svg?sanitize=true"/></td>
  </tr>
  <tr>
    <td>профайлер простого графа (арифметические операции и ветвление)</td><td>граф, вычисляющий n-ое число фибоначчи по наивному методу, вершина CB (CallBack) выполняет роль рекурсии</td>
  </tr>
</table>

## Идеи

* Ввести понятие - подпространство. Это изолированная область от внешних и внутренних контекстов. У подпространства есть входные связи и выходные. Такой механизм позволит сделать рекурсию через генератор подпрастранства.
