--
-- PostgreSQL database dump
--

-- Dumped from database version 17.2
-- Dumped by pg_dump version 17.2

-- Started on 2025-03-03 01:28:36

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 2 (class 3079 OID 16457)
-- Name: uuid-ossp; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;


--
-- TOC entry 4846 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION "uuid-ossp"; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';


--
-- TOC entry 227 (class 1255 OID 16430)
-- Name: update_balance_on_currency_change(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.update_balance_on_currency_change() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF (TG_OP = 'UPDATE' AND NEW.currency_type != OLD.currency_type) THEN
        IF (NEW.currency_type = 'рубли') THEN
            NEW.balance := OLD.balance / get_exchange_rate(); -- конвертация в рубли
        ELSIF (NEW.currency_type = 'у.е.') THEN
            NEW.balance := OLD.balance * get_exchange_rate(); -- конвертация в у.е.
        END IF;
    END IF;

    RETURN NEW; -- Важно вернуть NEW запись для дальнейшей обработки
END;
$$;


ALTER FUNCTION public.update_balance_on_currency_change() OWNER TO postgres;

--
-- TOC entry 225 (class 1255 OID 16425)
-- Name: update_user_account_count(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.update_user_account_count() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
  IF (TG_OP = 'INSERT') THEN
        UPDATE users SET account_count = account_count + 1 WHERE user_id = NEW.user_id;
    ELSIF (TG_OP = 'DELETE') THEN
        UPDATE users SET account_count = account_count - 1 WHERE user_id = OLD.user_id;
     ELSIF (TG_OP = 'UPDATE' AND NEW.user_id != OLD.user_id) THEN
       UPDATE users SET account_count = account_count - 1 WHERE user_id = OLD.user_id;
      UPDATE users SET account_count = account_count + 1 WHERE user_id = NEW.user_id;
  END IF;

  RETURN NULL;
END;
$$;


ALTER FUNCTION public.update_user_account_count() OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 221 (class 1259 OID 16410)
-- Name: accounts; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.accounts (
    account_id integer NOT NULL,
    user_id integer NOT NULL,
    currency_type character varying(20) DEFAULT 'рубли'::character varying NOT NULL,
    balance numeric(18,2) DEFAULT 0.00 NOT NULL,
    CONSTRAINT currency_type_check CHECK (((currency_type)::text = ANY ((ARRAY['рубли'::character varying, 'у.е.'::character varying])::text[]))),
    CONSTRAINT positive_balance CHECK ((balance >= (0)::numeric))
);


ALTER TABLE public.accounts OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 16409)
-- Name: accounts_account_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.accounts_account_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.accounts_account_id_seq OWNER TO postgres;

--
-- TOC entry 4847 (class 0 OID 0)
-- Dependencies: 220
-- Name: accounts_account_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.accounts_account_id_seq OWNED BY public.accounts.account_id;


--
-- TOC entry 223 (class 1259 OID 16433)
-- Name: transfers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.transfers (
    transfer_id integer NOT NULL,
    account_id integer NOT NULL,
    appointment_account_id integer NOT NULL,
    currency_type character varying(20) NOT NULL,
    transfer_date timestamp with time zone NOT NULL,
    transfer_sum numeric NOT NULL,
    CONSTRAINT currency_type_check CHECK (((currency_type)::text = ANY ((ARRAY['рубли'::character varying, 'у.е.'::character varying])::text[])))
);


ALTER TABLE public.transfers OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 16432)
-- Name: transfers_transfer_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.transfers_transfer_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.transfers_transfer_id_seq OWNER TO postgres;

--
-- TOC entry 4848 (class 0 OID 0)
-- Dependencies: 222
-- Name: transfers_transfer_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.transfers_transfer_id_seq OWNED BY public.transfers.transfer_id;


--
-- TOC entry 219 (class 1259 OID 16389)
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    user_id integer NOT NULL,
    first_name character varying(255) NOT NULL,
    last_name character varying(255) NOT NULL,
    middle_name character varying(255),
    email character varying(255) NOT NULL,
    password character varying(255) NOT NULL,
    account_count integer DEFAULT 0 NOT NULL,
    CONSTRAINT account_count_check CHECK (((account_count >= 0) AND (account_count <= 5)))
);


ALTER TABLE public.users OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 16388)
-- Name: users_user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.users_user_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.users_user_id_seq OWNER TO postgres;

--
-- TOC entry 4849 (class 0 OID 0)
-- Dependencies: 218
-- Name: users_user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_user_id_seq OWNED BY public.users.user_id;


--
-- TOC entry 4666 (class 2604 OID 16413)
-- Name: accounts account_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.accounts ALTER COLUMN account_id SET DEFAULT nextval('public.accounts_account_id_seq'::regclass);


--
-- TOC entry 4669 (class 2604 OID 16436)
-- Name: transfers transfer_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transfers ALTER COLUMN transfer_id SET DEFAULT nextval('public.transfers_transfer_id_seq'::regclass);


--
-- TOC entry 4664 (class 2604 OID 16392)
-- Name: users user_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN user_id SET DEFAULT nextval('public.users_user_id_seq'::regclass);


--
-- TOC entry 4838 (class 0 OID 16410)
-- Dependencies: 221
-- Data for Name: accounts; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.accounts (account_id, user_id, currency_type, balance) FROM stdin;
2	2	у.е.	100.00
1	2	рубли	9900.00
1737294767	387017244	рубли	13100.00
1664359581	2	рубли	1000.00
1090750812	2	рубли	1000.00
1431079565	571622159	рубли	1111.00
\.


--
-- TOC entry 4840 (class 0 OID 16433)
-- Dependencies: 223
-- Data for Name: transfers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.transfers (transfer_id, account_id, appointment_account_id, currency_type, transfer_date, transfer_sum) FROM stdin;
3214	1	2	рубли	2001-10-21 00:00:00+04	111.00
962631577	1	1	рубли	2025-02-27 04:03:06.6+03	100
131055505	1664359581	1664359581	рубли	2025-02-27 09:42:35.512+03	100
1544293852	1664359581	1664359581	рубли	2025-02-27 09:42:35.512+03	100
1561054968	2	2	рубли	2025-02-27 10:08:42.186+03	100
\.


--
-- TOC entry 4836 (class 0 OID 16389)
-- Dependencies: 219
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (user_id, first_name, last_name, middle_name, email, password, account_count) FROM stdin;
1978635480	Иван	Иванов		KrutoiAddres@yandex.ru	hype	5
387017244	string	string	string	string	string	1
2	T	T	\N	T@Mail	ttt	4
571622159	Киррилл	Клюшанов	Владимирович	string@	string	1
\.


--
-- TOC entry 4850 (class 0 OID 0)
-- Dependencies: 220
-- Name: accounts_account_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.accounts_account_id_seq', 1, true);


--
-- TOC entry 4851 (class 0 OID 0)
-- Dependencies: 222
-- Name: transfers_transfer_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.transfers_transfer_id_seq', 1, false);


--
-- TOC entry 4852 (class 0 OID 0)
-- Dependencies: 218
-- Name: users_user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_user_id_seq', 2, true);


--
-- TOC entry 4679 (class 2606 OID 16419)
-- Name: accounts accounts_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.accounts
    ADD CONSTRAINT accounts_pkey PRIMARY KEY (account_id);


--
-- TOC entry 4683 (class 2606 OID 16439)
-- Name: transfers transfers_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transfers
    ADD CONSTRAINT transfers_pkey PRIMARY KEY (transfer_id);


--
-- TOC entry 4675 (class 2606 OID 16400)
-- Name: users users_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_email_key UNIQUE (email);


--
-- TOC entry 4677 (class 2606 OID 16398)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (user_id);


--
-- TOC entry 4680 (class 1259 OID 16450)
-- Name: idx_transfers_account_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_transfers_account_id ON public.transfers USING btree (account_id);


--
-- TOC entry 4681 (class 1259 OID 16451)
-- Name: idx_transfers_appointment_account_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_transfers_appointment_account_id ON public.transfers USING btree (appointment_account_id);


--
-- TOC entry 4687 (class 2620 OID 16427)
-- Name: accounts accounts_after_delete; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER accounts_after_delete AFTER DELETE ON public.accounts FOR EACH ROW EXECUTE FUNCTION public.update_user_account_count();


--
-- TOC entry 4688 (class 2620 OID 16426)
-- Name: accounts accounts_after_insert; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER accounts_after_insert AFTER INSERT ON public.accounts FOR EACH ROW EXECUTE FUNCTION public.update_user_account_count();


--
-- TOC entry 4689 (class 2620 OID 16428)
-- Name: accounts accounts_after_update; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER accounts_after_update AFTER UPDATE ON public.accounts FOR EACH ROW EXECUTE FUNCTION public.update_user_account_count();


--
-- TOC entry 4684 (class 2606 OID 16420)
-- Name: accounts fk_accounts_user_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.accounts
    ADD CONSTRAINT fk_accounts_user_id FOREIGN KEY (user_id) REFERENCES public.users(user_id) ON DELETE CASCADE;


--
-- TOC entry 4685 (class 2606 OID 16440)
-- Name: transfers fk_transfers_account_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transfers
    ADD CONSTRAINT fk_transfers_account_id FOREIGN KEY (account_id) REFERENCES public.accounts(account_id) ON DELETE CASCADE;


--
-- TOC entry 4686 (class 2606 OID 16445)
-- Name: transfers fk_transfers_appointment_account_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.transfers
    ADD CONSTRAINT fk_transfers_appointment_account_id FOREIGN KEY (appointment_account_id) REFERENCES public.accounts(account_id) ON DELETE CASCADE;


-- Completed on 2025-03-03 01:28:36

--
-- PostgreSQL database dump complete
--

