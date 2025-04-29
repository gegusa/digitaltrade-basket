create schema basket

CREATE TABLE if not exists basket.shopping_cart_items
(
    id              BIGSERIAL         PRIMARY KEY,
    client_id       BIGSERIAL         NOT NULL,                      -- Идентификатор пользователя
    product_id      BIGSERIAL         NOT NULL,                      -- Идентификатор товара
    name            TEXT              NOT NULL,                      -- Название товара
    quantity        INT               NOT NULL CHECK (quantity > 0), -- Количество товара
    price_at_adding NUMERIC(10, 2)    NOT NULL,                      -- Цена товара на момент добавления
    added_at        TIMESTAMP WITHOUT TIME ZONE DEFAULT now(),       -- Когда добавили в корзину
    updated_at      TIMESTAMP WITHOUT TIME ZONE DEFAULT now(),       -- Когда обновили (например, изменили количество)

    -- Ограничение: у одного пользователя не может быть несколько одинаковых товаров в корзине
    CONSTRAINT unique_user_product UNIQUE (client_id, product_id)
);