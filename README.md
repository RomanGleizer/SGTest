# SGTest

## Инструкция по использованию

### 1. Скачайте архив и распакуйте его
- Скачайте архив [Result.zip](https://github.com/RomanGleizer/SGTest/files/14219584/Result.zip) и распакуйте его на вашем устройстве.

### 2. Настройка файла `appsettings.json`
Откройте файл `appsettings.json` и настройте строку подключения для вашей базы данных.

### 3. Создание таблиц
Создайте таблицы в вашей базе данных, используя предоставленный SQL код.

```SQL
CREATE TABLE "JobTitle" (
    "ID" SERIAL PRIMARY KEY,
    "Name" VARCHAR(100)
);

CREATE TABLE "Departments" (
    "ID" SERIAL PRIMARY KEY,
    "Name" VARCHAR(100),
    "ParentID" INT,
    "ManagerID" INT,
    "Phone" VARCHAR(20)
);

CREATE TABLE "Employees" (
    "ID" SERIAL PRIMARY KEY,
    "DepartmentID" INT,
    "FullName" VARCHAR(100),
    "Login" VARCHAR(50),
    "Password" VARCHAR(50),
    "JobTitle" INT
);

ALTER TABLE "Departments"
    ADD CONSTRAINT "fk_parent_department"
    FOREIGN KEY ("ParentID")
    REFERENCES "Departments"("ID");

ALTER TABLE "Departments"
    ADD CONSTRAINT "fk_manager_employee"
    FOREIGN KEY ("ManagerID")
    REFERENCES "Employees"("ID");

ALTER TABLE "Employees"
    ADD CONSTRAINT "fk_department_employee"
    FOREIGN KEY ("DepartmentID")
    REFERENCES "Departments"("ID");

ALTER TABLE "Employees"
    ADD CONSTRAINT "fk_jobtitle_employee"
    FOREIGN KEY ("JobTitle")
    REFERENCES "JobTitle"("ID");
```

### 4. Запуск приложения
Найдите файл .exe в папке с проектом и запустите его. Это откроет консоль и приложение.

![Пример консольного запуска](https://github.com/RomanGleizer/SGTest/assets/125725530/55a39429-cf2a-4e35-a26f-08c982cbb90d)

### 5. Импорт данных
Для импорта данных нажмите на кнопку "Импорт данных" и выберите файл .tsv для импорта данных.

![Импорт данных](https://github.com/RomanGleizer/SGTest/assets/125725530/8712824e-98bb-4f3c-963b-b816d60863c9)

### 6. Обработка ошибок
Если файл не соответствует необходимому типу импорта, вы увидите соответствующую ошибку.

![Ошибка](https://github.com/RomanGleizer/SGTest/assets/125725530/0c1b0e76-f959-4761-8db9-07f3b39c56f9)

### 7. Успешный импорт
После успешного импорта данных вы увидите соответствующее сообщение в консоли.

![Успех](https://github.com/RomanGleizer/SGTest/assets/125725530/24cb148c-a61f-4228-b644-23bbcc72b743)

### 8. Вывод данных
Для вывода данных нажмите на кнопку "Вывод данных" на главной форме. Вы сможете выбрать тип вывода.

![Вывод данных](https://github.com/RomanGleizer/SGTest/assets/125725530/34da0ba9-3fa8-4cdd-b716-8491fdf351bf)

### 9. Пример вывода
Пример вывода информации о подразделениях:

![Пример вывода](https://github.com/RomanGleizer/SGTest/assets/125725530/c9be4e00-248f-4d46-9471-fb60c8719d6f)

## Важное уточнение в функционале программы

### Порядок импорта имеет значение
Столбцы таблицы могут принимать значение null, если программа не смогла найти таблицу, с которой планировалось установление связи. В этом случае поможет добавление таблицы, с которой вы планируете установить связь и повторной импорт данных для текущей таблицы. Программа не создаст повторяющиеся записи, а обновит внешние ключи и другие данные таблицы.
