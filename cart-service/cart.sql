CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE carts (
    id uuid NOT NULL,
    user_id uuid NOT NULL,
    status character varying(20) NOT NULL DEFAULT ('ACTIVE'::character varying),
    total_price numeric(12,2) NOT NULL,
    created_at timestamp without time zone NOT NULL DEFAULT (now()),
    updated_at timestamp without time zone NOT NULL DEFAULT (now()),
    CONSTRAINT carts_pkey PRIMARY KEY (id)
);

CREATE TABLE cart_discounts (
    id uuid NOT NULL,
    cart_id uuid NOT NULL,
    code character varying(50) NOT NULL,
    discount_type character varying(20) NOT NULL,
    discount_value numeric(12,2) NOT NULL,
    applied_amount numeric(12,2) NOT NULL,
    CONSTRAINT cart_discounts_pkey PRIMARY KEY (id),
    CONSTRAINT fk_cart_discount FOREIGN KEY (cart_id) REFERENCES carts (id) ON DELETE CASCADE
);

CREATE TABLE cart_items (
    id uuid NOT NULL,
    cart_id uuid NOT NULL,
    product_id uuid NOT NULL,
    variant_id uuid,
    product_name character varying(255) NOT NULL,
    variant_name character varying(255),
    unit_price numeric(12,2) NOT NULL,
    quantity integer NOT NULL,
    total_price numeric(12,2) NOT NULL,
    created_at timestamp without time zone NOT NULL DEFAULT (now()),
    updated_at timestamp without time zone NOT NULL DEFAULT (now()),
    CONSTRAINT cart_items_pkey PRIMARY KEY (id),
    CONSTRAINT fk_cart_items_cart FOREIGN KEY (cart_id) REFERENCES carts (id) ON DELETE CASCADE
);

CREATE INDEX "IX_cart_discounts_cart_id" ON cart_discounts (cart_id);

CREATE UNIQUE INDEX uq_cart_item_unique ON cart_items (cart_id, product_id, variant_id);

CREATE UNIQUE INDEX uq_cart_user_active ON carts (user_id) WHERE ((status)::text = 'ACTIVE'::text);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260205033903_v1', '8.0.22');

COMMIT;

