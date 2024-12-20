toc.dat                                                                                             0000600 0004000 0002000 00000012234 14612203433 0014437 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        PGDMP       2                |            Test    16.1    16.1     �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false         �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false         �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false         �           1262    42406    Test    DATABASE     y   CREATE DATABASE "Test" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_India.1252';
    DROP DATABASE "Test";
                postgres    false         �            1259    42415    User    TABLE     k  CREATE TABLE public."User" (
    userid integer NOT NULL,
    firstname character varying(100),
    lastname character varying(100),
    cityid integer NOT NULL,
    age integer,
    email character varying(255) NOT NULL,
    phoneno character varying(100),
    gender character varying(50),
    city character varying(100),
    country character varying(100)
);
    DROP TABLE public."User";
       public         heap    postgres    false         �            1259    42414    User_userid_seq    SEQUENCE     �   CREATE SEQUENCE public."User_userid_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public."User_userid_seq";
       public          postgres    false    218         �           0    0    User_userid_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public."User_userid_seq" OWNED BY public."User".userid;
          public          postgres    false    217         �            1259    42408    city    TABLE     W   CREATE TABLE public.city (
    id integer NOT NULL,
    name character varying(100)
);
    DROP TABLE public.city;
       public         heap    postgres    false         �            1259    42407    city_id_seq    SEQUENCE     �   CREATE SEQUENCE public.city_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 "   DROP SEQUENCE public.city_id_seq;
       public          postgres    false    216         �           0    0    city_id_seq    SEQUENCE OWNED BY     ;   ALTER SEQUENCE public.city_id_seq OWNED BY public.city.id;
          public          postgres    false    215         W           2604    42418    User userid    DEFAULT     n   ALTER TABLE ONLY public."User" ALTER COLUMN userid SET DEFAULT nextval('public."User_userid_seq"'::regclass);
 <   ALTER TABLE public."User" ALTER COLUMN userid DROP DEFAULT;
       public          postgres    false    217    218    218         V           2604    42411    city id    DEFAULT     b   ALTER TABLE ONLY public.city ALTER COLUMN id SET DEFAULT nextval('public.city_id_seq'::regclass);
 6   ALTER TABLE public.city ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    215    216    216         �          0    42415    User 
   TABLE DATA           q   COPY public."User" (userid, firstname, lastname, cityid, age, email, phoneno, gender, city, country) FROM stdin;
    public          postgres    false    218       4849.dat �          0    42408    city 
   TABLE DATA           (   COPY public.city (id, name) FROM stdin;
    public          postgres    false    216       4847.dat �           0    0    User_userid_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('public."User_userid_seq"', 17, true);
          public          postgres    false    217         �           0    0    city_id_seq    SEQUENCE SET     9   SELECT pg_catalog.setval('public.city_id_seq', 9, true);
          public          postgres    false    215         [           2606    42424    User User_email_key 
   CONSTRAINT     S   ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_email_key" UNIQUE (email);
 A   ALTER TABLE ONLY public."User" DROP CONSTRAINT "User_email_key";
       public            postgres    false    218         ]           2606    42422    User User_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_pkey" PRIMARY KEY (userid);
 <   ALTER TABLE ONLY public."User" DROP CONSTRAINT "User_pkey";
       public            postgres    false    218         Y           2606    42413    city city_pkey 
   CONSTRAINT     L   ALTER TABLE ONLY public.city
    ADD CONSTRAINT city_pkey PRIMARY KEY (id);
 8   ALTER TABLE ONLY public.city DROP CONSTRAINT city_pkey;
       public            postgres    false    216         ^           2606    42425    User User_cityid_fkey    FK CONSTRAINT     v   ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_cityid_fkey" FOREIGN KEY (cityid) REFERENCES public.city(id);
 C   ALTER TABLE ONLY public."User" DROP CONSTRAINT "User_cityid_fkey";
       public          postgres    false    216    4697    218                                                                                                                                                                                                                                                                                                                                                                            4849.dat                                                                                            0000600 0004000 0002000 00000001620 14612203433 0014257 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        1	John	Smith	1	30	john@example.com	9875484578	Male	Anand	India
2	Alice	Johnson	2	25	alice@example.com	987-654-3210	Female	Ahmedabad	India
3	Michael	Brown	3	35	michael@example.com	111-222-3333	Male	Vadodara	India
4	Emily	Davis	4	28	emily@example.com	444-555-6666	Female	Surat	India
5	David	Garcia	5	32	david@example.com	777-888-9999	Male	Bharuch	India
6	Sarah	Martinez	1	27	sarah@example.com	555-666-7777	Female	Anand	India
7	James	Rodriguez	3	40	james@example.com	222-333-4444	Male	Vadodara	India
8	Jessica	Wilson	2	29	jessica@example.com	888-999-0000	Female	Ahmedabad	India
9	Daniel	Taylor	4	34	daniel@example.com	333-444-5555	Male	Surat	India
12	vraj	parekh	1	22	v@gmail.com	9426161710	Male	anand	india
14	dshd	dsfh	7	20	vraj@gmail.com	9426161710	Male	rajkot	india
15	raj	patel	7	21	user@gmail.com	65465465454	Male	rajkot	india
17	asdf	lastname	9	20	patient@gmail.com	+915454545454	Male	banaskantha	india
\.


                                                                                                                4847.dat                                                                                            0000600 0004000 0002000 00000000142 14612203433 0014253 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        1	Anand
2	Ahmedabad
3	Vadodara
4	Surat
5	Bharuch
6	amreli
7	rajkot
8	porbandar
9	banaskantha
\.


                                                                                                                                                                                                                                                                                                                                                                                                                              restore.sql                                                                                         0000600 0004000 0002000 00000011166 14612203433 0015367 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        --
-- NOTE:
--
-- File paths need to be edited. Search for $$PATH$$ and
-- replace it with the path to the directory containing
-- the extracted data files.
--
--
-- PostgreSQL database dump
--

-- Dumped from database version 16.1
-- Dumped by pg_dump version 16.1

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

DROP DATABASE "Test";
--
-- Name: Test; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE "Test" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_India.1252';


ALTER DATABASE "Test" OWNER TO postgres;

\connect "Test"

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
-- Name: User; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."User" (
    userid integer NOT NULL,
    firstname character varying(100),
    lastname character varying(100),
    cityid integer NOT NULL,
    age integer,
    email character varying(255) NOT NULL,
    phoneno character varying(100),
    gender character varying(50),
    city character varying(100),
    country character varying(100)
);


ALTER TABLE public."User" OWNER TO postgres;

--
-- Name: User_userid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."User_userid_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."User_userid_seq" OWNER TO postgres;

--
-- Name: User_userid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."User_userid_seq" OWNED BY public."User".userid;


--
-- Name: city; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.city (
    id integer NOT NULL,
    name character varying(100)
);


ALTER TABLE public.city OWNER TO postgres;

--
-- Name: city_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.city_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.city_id_seq OWNER TO postgres;

--
-- Name: city_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.city_id_seq OWNED BY public.city.id;


--
-- Name: User userid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."User" ALTER COLUMN userid SET DEFAULT nextval('public."User_userid_seq"'::regclass);


--
-- Name: city id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.city ALTER COLUMN id SET DEFAULT nextval('public.city_id_seq'::regclass);


--
-- Data for Name: User; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."User" (userid, firstname, lastname, cityid, age, email, phoneno, gender, city, country) FROM stdin;
\.
COPY public."User" (userid, firstname, lastname, cityid, age, email, phoneno, gender, city, country) FROM '$$PATH$$/4849.dat';

--
-- Data for Name: city; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.city (id, name) FROM stdin;
\.
COPY public.city (id, name) FROM '$$PATH$$/4847.dat';

--
-- Name: User_userid_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."User_userid_seq"', 17, true);


--
-- Name: city_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.city_id_seq', 9, true);


--
-- Name: User User_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_email_key" UNIQUE (email);


--
-- Name: User User_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_pkey" PRIMARY KEY (userid);


--
-- Name: city city_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.city
    ADD CONSTRAINT city_pkey PRIMARY KEY (id);


--
-- Name: User User_cityid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_cityid_fkey" FOREIGN KEY (cityid) REFERENCES public.city(id);


--
-- PostgreSQL database dump complete
--

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          