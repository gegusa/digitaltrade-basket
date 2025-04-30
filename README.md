# 🧺 DigitalTrade Basket Service

Basket Service отвечает за управление корзиной покупателя в рамках микросервисной архитектуры интернет-магазина **DigitalTrade**.

## 📌 Основные возможности

- Добавление товара в корзину
- Изменение количества товара
- Удаление всей корзины
- Получение текущего состояния корзины клиента
- Оформление заказа (checkout) с отправкой события в Kafka

## 🚀 REST API

Базовый путь: `/api/basket`

| Метод  | Путь             | Описание                              |
|--------|------------------|----------------------------------------|
| `GET`  | `/get`           | Получить корзину по `clientId`        |
| `POST` | `/add-item`      | Добавить товар в корзину              |
| `POST` | `/change-quantity` | Изменить количество товара в корзине  |
| `POST` | `/delete`        | Удалить корзину                       |
| `POST` | `/checkout`      | Оформить корзину (checkout)           |

> Параметры и тела запросов/ответов определяются в соответствующих классах из пространства имён `DigitalTrade.Basket.Api.Contracts`.

## 🧩 Взаимодействие с другими микросервисами

### Событийное взаимодействие через Kafka

#### 📤 При оформлении заказа

После вызова `/checkout` сервис отправляет в Kafka событие:

- **Топик:** `digitaltrade.ordering.checkouts`
- **Событие:** `BasketCheckoutRequestedEvent`

Это событие потребляется **DigitalTrade.Ordering** для создания заказа.

#### 📥 При изменении или удалении товара

Сервис подписан на следующие Kafka топики:

- `digitaltrade.catalog.changed` → `ProductDeletedEvent`
- `digitaltrade.catalog.changed` → `ProductUpdatedEvent`

На основе этих событий корзина автоматически обновляется: удаляются или изменяются товары, недоступные для покупки.

## 🖼️ Архитектурная схема

```mermaid
graph TD
    subgraph
        UI[Client / Frontend]
        UI --> |REST| BasketService
    end

    subgraph
        BasketService[[Basket Service]]
        BasketService -->|Kafka Event: CheckoutRequestedEvent| Kafka
        Kafka --> OrderingService
        CatalogService -->|Kafka Event: ProductDeletedEvent / ProductUpdatedEvent| Kafka
        Kafka --> BasketService
    end

    subgraph
        OrderingService[[Ordering Service]]
        CatalogService[[Catalog Service]]
    end
