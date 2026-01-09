CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE categories (
    id uuid NOT NULL DEFAULT (uuidv7()),
    name character varying(100) NOT NULL,
    description text,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    updated_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    CONSTRAINT categories_pkey PRIMARY KEY (id)
);

CREATE TABLE products (
    id uuid NOT NULL DEFAULT (uuidv7()),
    name character varying(150) NOT NULL,
    description text,
    price numeric(18,2) NOT NULL,
    category_id uuid NOT NULL,
    is_available boolean NOT NULL DEFAULT TRUE,
    is_deleted boolean NOT NULL DEFAULT FALSE,
    created_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    updated_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    CONSTRAINT products_pkey PRIMARY KEY (id),
    CONSTRAINT fk_products_category FOREIGN KEY (category_id) REFERENCES categories (id)
);

CREATE TABLE product_images (
    id uuid NOT NULL DEFAULT (uuidv7()),
    product_id uuid NOT NULL,
    image_url text NOT NULL,
    is_main boolean NOT NULL DEFAULT FALSE,
    CONSTRAINT product_images_pkey PRIMARY KEY (id),
    CONSTRAINT fk_image_product FOREIGN KEY (product_id) REFERENCES products (id) ON DELETE CASCADE
);

CREATE TABLE product_variants (
    id uuid NOT NULL DEFAULT (uuidv7()),
    product_id uuid NOT NULL,
    name character varying(100) NOT NULL,
    extra_price numeric(18,2) NOT NULL,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    updated_at timestamp with time zone NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    CONSTRAINT product_variants_pkey PRIMARY KEY (id),
    CONSTRAINT fk_variant_product FOREIGN KEY (product_id) REFERENCES products (id) ON DELETE CASCADE
);

CREATE INDEX "IX_product_images_product_id" ON product_images (product_id);

CREATE INDEX "IX_product_variants_product_id" ON product_variants (product_id);

CREATE INDEX "IX_products_category_id" ON products (category_id);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260107085405_v0', '8.0.22');

COMMIT;

