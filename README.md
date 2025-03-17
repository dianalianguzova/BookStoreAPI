# Описание API
BookStoreAPI - api, предоставляющее функциональность книжного интернет-магазина.

# ER-диаграмма
![ER-диаграмма](ER.png)

На диаграмме представлена структура таблиц базы данных.

# Маршруты API

## Маршруты BookProduct

### GET
- **Получение списка всех книжных продуктов**: 
  - `GET https://localhost:5001/`
- **Получение списка всех книг**: 
  - `GET https://localhost:5001/books`
- **Получение списка всех комиксов**: 
  - `GET https://localhost:5001/comics`
- **Получение информации об одном книжном продукте по ID**: 
  - `GET https://localhost:5001/product/{id}`

### POST
- **Добавление нового книжного продукта**: 
  - `POST https://localhost:5001/`

### PUT
- **Изменение книжного продукта по ID**: 
  - `PUT https://localhost:5001/products/{id}`

### DELETE
- **Удаление книжного продукта по ID**: 
  - `DELETE https://localhost:5001/products/{id}`
