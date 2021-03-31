--
-- PostgreSQL database dump
--

-- Dumped from database version 13.2
-- Dumped by pg_dump version 13.2

-- Started on 2021-03-31 20:39:58

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 201 (class 1259 OID 16397)
-- Name: contacts; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.contacts (
    id integer NOT NULL,
    name text NOT NULL,
    date_of_birth date NOT NULL,
    address text NOT NULL
);


ALTER TABLE public.contacts OWNER TO postgres;

--
-- TOC entry 200 (class 1259 OID 16395)
-- Name: contacts_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.contacts_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.contacts_id_seq OWNER TO postgres;

--
-- TOC entry 3015 (class 0 OID 0)
-- Dependencies: 200
-- Name: contacts_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.contacts_id_seq OWNED BY public.contacts.id;


--
-- TOC entry 205 (class 1259 OID 16458)
-- Name: contactswithnumbers; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.contactswithnumbers AS
SELECT
    NULL::integer AS id,
    NULL::text AS name,
    NULL::date AS date_of_birth,
    NULL::text AS address,
    NULL::text AS numbers;


ALTER TABLE public.contactswithnumbers OWNER TO postgres;

--
-- TOC entry 204 (class 1259 OID 16411)
-- Name: phone_numbers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.phone_numbers (
    id integer NOT NULL,
    contacts_id integer NOT NULL,
    number text NOT NULL
);


ALTER TABLE public.phone_numbers OWNER TO postgres;

--
-- TOC entry 203 (class 1259 OID 16409)
-- Name: phone_numbers_contacts_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.phone_numbers_contacts_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.phone_numbers_contacts_id_seq OWNER TO postgres;

--
-- TOC entry 3016 (class 0 OID 0)
-- Dependencies: 203
-- Name: phone_numbers_contacts_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.phone_numbers_contacts_id_seq OWNED BY public.phone_numbers.contacts_id;


--
-- TOC entry 202 (class 1259 OID 16407)
-- Name: phone_numbers_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.phone_numbers_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.phone_numbers_id_seq OWNER TO postgres;

--
-- TOC entry 3017 (class 0 OID 0)
-- Dependencies: 202
-- Name: phone_numbers_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.phone_numbers_id_seq OWNED BY public.phone_numbers.id;


--
-- TOC entry 2864 (class 2604 OID 16400)
-- Name: contacts id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.contacts ALTER COLUMN id SET DEFAULT nextval('public.contacts_id_seq'::regclass);


--
-- TOC entry 2865 (class 2604 OID 16414)
-- Name: phone_numbers id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.phone_numbers ALTER COLUMN id SET DEFAULT nextval('public.phone_numbers_id_seq'::regclass);


--
-- TOC entry 2866 (class 2604 OID 16415)
-- Name: phone_numbers contacts_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.phone_numbers ALTER COLUMN contacts_id SET DEFAULT nextval('public.phone_numbers_contacts_id_seq'::regclass);


--
-- TOC entry 3006 (class 0 OID 16397)
-- Dependencies: 201
-- Data for Name: contacts; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.contacts (id, name, date_of_birth, address) FROM stdin;
\.


--
-- TOC entry 3009 (class 0 OID 16411)
-- Dependencies: 204
-- Data for Name: phone_numbers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.phone_numbers (id, contacts_id, number) FROM stdin;
\.


--
-- TOC entry 3018 (class 0 OID 0)
-- Dependencies: 200
-- Name: contacts_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.contacts_id_seq', 12, true);


--
-- TOC entry 3019 (class 0 OID 0)
-- Dependencies: 203
-- Name: phone_numbers_contacts_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.phone_numbers_contacts_id_seq', 1, true);


--
-- TOC entry 3020 (class 0 OID 0)
-- Dependencies: 202
-- Name: phone_numbers_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.phone_numbers_id_seq', 24, true);


--
-- TOC entry 2868 (class 2606 OID 16402)
-- Name: contacts contacts_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.contacts
    ADD CONSTRAINT contacts_pkey PRIMARY KEY (id);


--
-- TOC entry 2870 (class 2606 OID 16463)
-- Name: contacts name_and_address; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.contacts
    ADD CONSTRAINT name_and_address UNIQUE (name, address);


--
-- TOC entry 2872 (class 2606 OID 16417)
-- Name: phone_numbers phone_numbers_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.phone_numbers
    ADD CONSTRAINT phone_numbers_pkey PRIMARY KEY (id);


--
-- TOC entry 3004 (class 2618 OID 16461)
-- Name: contactswithnumbers _RETURN; Type: RULE; Schema: public; Owner: postgres
--

CREATE OR REPLACE VIEW public.contactswithnumbers AS
 SELECT c.id,
    c.name,
    c.date_of_birth,
    c.address,
    string_agg(p.number, ';'::text) AS numbers
   FROM (public.contacts c
     JOIN public.phone_numbers p ON ((c.id = p.contacts_id)))
  GROUP BY c.id;


--
-- TOC entry 2873 (class 2606 OID 16418)
-- Name: phone_numbers contacts_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.phone_numbers
    ADD CONSTRAINT contacts_id FOREIGN KEY (contacts_id) REFERENCES public.contacts(id);


-- Completed on 2021-03-31 20:39:58

--
-- PostgreSQL database dump complete
--

