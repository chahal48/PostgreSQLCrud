-- Type: udt_category

-- DROP TYPE IF EXISTS public.udt_category;

CREATE TYPE public.udt_category AS ENUM
    ('Client', 'Vendor');

-- Type: udt_gender

-- DROP TYPE IF EXISTS public.udt_gender;

CREATE TYPE public.udt_gender AS ENUM
    ('Male', 'Female', 'Other');
	
-- Table: public.tblcontacts

-- DROP TABLE IF EXISTS public.tblcontacts;

CREATE TABLE IF NOT EXISTS public.tblcontacts
(
    contactid serial NOT NULL PRIMARY KEY,
    professionid integer NOT NULL,
    firstname character varying(30) NOT NULL,
    lastname character varying(30),
    emailaddress character varying(100) UNIQUE NOT NULL,
    company character varying(50) NOT NULL,
    category udt_category NOT NULL,
    gender udt_gender NOT NULL,
    dob timestamp without time zone NOT NULL,
    modeslack boolean NOT NULL,
    modewhatsapp boolean NOT NULL,
    modeemail boolean NOT NULL,
    modephone boolean NOT NULL,
    contactimage text,
    isdeleted boolean NOT NULL DEFAULT false,
    createdon timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    lastmodified timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
)

-- Table: public.tblprofessions

-- DROP TABLE IF EXISTS public.tblprofessions;

CREATE TABLE IF NOT EXISTS public.tblprofessions
(
    professionid serial NOT NULL PRIMARY KEY,
    profession character varying(100) UNIQUE NOT NULL,
    description character varying(400),
    isdeleted boolean NOT NULL DEFAULT false,
    createdon timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    lastmodified timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
)

-- FUNCTION: public.udf_addpofession(character varying, character varying)

-- DROP FUNCTION IF EXISTS public.udf_addpofession(character varying, character varying);

CREATE OR REPLACE FUNCTION public.udf_addpofession(
	_profession character varying,
	_description character varying)
    RETURNS integer
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
BEGIN
	INSERT INTO tblProfessions(Profession,Description)
	VALUES(_Profession,_Description);
	IF FOUND THEN -- INSERTED SUCCESSFULLY
	RETURN 1;
	ELSE RETURN 0; -- INSERTED FAIL
	END IF;																	
END
$BODY$;

-- FUNCTION: public.udf_checkemailalreadyexists(character varying)

-- DROP FUNCTION IF EXISTS public.udf_checkemailalreadyexists(character varying);

CREATE OR REPLACE FUNCTION public.udf_checkemailalreadyexists(
	_email character varying)
    RETURNS integer
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
DECLARE result integer;
begin
	result:= count(contactid) FROM public.tblcontacts where LOWER(TRIM(emailaddress)) = LOWER(TRIM(_email));
	return result;
end
$BODY$;

-- FUNCTION: public.udf_checkprofessionalreadyexists(character varying)

-- DROP FUNCTION IF EXISTS public.udf_checkprofessionalreadyexists(character varying);

CREATE OR REPLACE FUNCTION public.udf_checkprofessionalreadyexists(
	_profession character varying)
    RETURNS integer
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
DECLARE result integer;
begin
	result:=  count(ProfessionId) FROM tblProfessions where LOWER(TRIM(Profession)) = LOWER(TRIM(_Profession));
	return result;
end
$BODY$;

-- FUNCTION: public.udf_deletecontact(integer)

-- DROP FUNCTION IF EXISTS public.udf_deletecontact(integer);

CREATE OR REPLACE FUNCTION public.udf_deletecontact(
	_cid integer)
    RETURNS integer
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
BEGIN
	UPDATE public.tblcontacts
	SET isdeleted=true, 
	lastmodified=default
	WHERE contactid = _cid;
	IF FOUND THEN -- delete SUCCESSFULLY
	RETURN 1;
	ELSE RETURN 0; -- delete FAIL
	END IF;																	
END
$BODY$;

-- FUNCTION: public.udf_deleteprofession(integer)

-- DROP FUNCTION IF EXISTS public.udf_deleteprofession(integer);

CREATE OR REPLACE FUNCTION public.udf_deleteprofession(
	_pid integer)
    RETURNS integer
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
BEGIN
	UPDATE public.tblprofessions
	SET isdeleted= true, lastmodified = default
	WHERE professionid = _pId;
	IF FOUND THEN -- Deleted SUCCESSFULLY
	RETURN 1;
	ELSE RETURN 0; -- Deleted FAIL
	END IF;																	
END
$BODY$;

-- FUNCTION: public.udf_getallcontact()

-- DROP FUNCTION IF EXISTS public.udf_getallcontact();

CREATE OR REPLACE FUNCTION public.udf_getallcontact(
	)
    RETURNS TABLE(_id integer, _professionid integer, _profession character varying, _firstname character varying, _lastname character varying, _emailaddress character varying, _company character varying, _category udt_category, _gender udt_gender, _dob timestamp without time zone, _modeslack boolean, _modewhatsapp boolean, _modeemail boolean, _modephone boolean, _contactimage text, _isdeleted boolean, _createdon timestamp without time zone, _lastmodified timestamp without time zone) 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$
begin
	Return query
	SELECT contactid, 
	tblcontacts.professionid,
	tblprofessions.profession,
	firstname, lastname, emailaddress,
	company, category, gender, dob, modeslack,
	modewhatsapp, modeemail, modephone, contactimage,
	tblcontacts.isdeleted, 
	tblcontacts.createdon, 
	tblcontacts.lastmodified
	FROM public.tblcontacts
	join 
	public.tblprofessions
	on tblprofessions.professionid = tblcontacts.professionid
	where tblcontacts.isdeleted = false;
end
$BODY$;

-- FUNCTION: public.udf_getallprofession()

-- DROP FUNCTION IF EXISTS public.udf_getallprofession();

CREATE OR REPLACE FUNCTION public.udf_getallprofession(
	)
    RETURNS TABLE(_id integer, _profession character varying, _description character varying, _lastmodified timestamp without time zone) 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$
begin
	Return query
	SELECT professionid, profession, description, lastmodified
	FROM public.tblprofessions
	where isdeleted = false;
end
$BODY$;

-- FUNCTION: public.udf_getcontactbyid(integer)

-- DROP FUNCTION IF EXISTS public.udf_getcontactbyid(integer);

CREATE OR REPLACE FUNCTION public.udf_getcontactbyid(
	_cid integer)
    RETURNS TABLE(_id integer, _professionid integer, _profession character varying, _firstname character varying, _lastname character varying, _emailaddress character varying, _company character varying, _category udt_category, _gender udt_gender, _dob timestamp without time zone, _modeslack boolean, _modewhatsapp boolean, _modeemail boolean, _modephone boolean, _contactimage text, _isdeleted boolean, _createdon timestamp without time zone, _lastmodified timestamp without time zone) 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$
begin
	Return query
	SELECT contactid, 
	tblcontacts.professionid,
	tblprofessions.profession,
	firstname, lastname, emailaddress,
	company, category, gender, dob, modeslack,
	modewhatsapp, modeemail, modephone, contactimage,
	tblcontacts.isdeleted, 
	tblcontacts.createdon, 
	tblcontacts.lastmodified
	FROM public.tblcontacts
	join 
	public.tblprofessions
	on tblprofessions.professionid = tblcontacts.professionid
	where tblcontacts.isdeleted = false and contactid = _cid;
end
$BODY$;

-- FUNCTION: public.udf_getprofessionbyid(integer)

-- DROP FUNCTION IF EXISTS public.udf_getprofessionbyid(integer);

CREATE OR REPLACE FUNCTION public.udf_getprofessionbyid(
	_pid integer)
    RETURNS TABLE(_id integer, _profession character varying, _description character varying, _lastmodified timestamp without time zone) 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$
begin
	Return query
	SELECT professionid, profession, description, lastmodified
	FROM public.tblprofessions
	where isdeleted = false and professionid = _pId;
end
$BODY$;

-- FUNCTION: public.udf_insertcontact(integer, character varying, character varying, character varying, character varying, character varying, character varying, timestamp without time zone, boolean, boolean, boolean, boolean, character varying)

-- DROP FUNCTION IF EXISTS public.udf_insertcontact(integer, character varying, character varying, character varying, character varying, character varying, character varying, timestamp without time zone, boolean, boolean, boolean, boolean, character varying);

CREATE OR REPLACE FUNCTION public.udf_insertcontact(
	_pid integer,
	_firstname character varying,
	_lastname character varying,
	_emailaddress character varying,
	_company character varying,
	_category character varying,
	_gender character varying,
	_dob timestamp without time zone,
	_modeslack boolean,
	_modewhatsapp boolean,
	_modeemail boolean,
	_modephone boolean,
	_contactimage character varying)
    RETURNS integer
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
BEGIN
	INSERT INTO public.tblcontacts(
	 professionid, firstname, lastname, emailaddress, company, category, gender, dob, modeslack, modewhatsapp, modeemail, modephone, contactimage)
	VALUES ( 
		_pid, _firstname, _lastname, _emailaddress, _company, _category::udt_category, _gender::udt_gender, _dob, _modeslack, _modewhatsapp, _modeemail, _modephone, _contactimage);
	IF FOUND THEN -- INSERTED SUCCESSFULLY
	RETURN 1;
	ELSE RETURN 0; -- INSERTED FAIL
	END IF;																	
END
$BODY$;

-- FUNCTION: public.udf_updatecontact(integer, integer, character varying, character varying, character varying, character varying, character varying, character varying, timestamp without time zone, boolean, boolean, boolean, boolean, character varying)

-- DROP FUNCTION IF EXISTS public.udf_updatecontact(integer, integer, character varying, character varying, character varying, character varying, character varying, character varying, timestamp without time zone, boolean, boolean, boolean, boolean, character varying);

CREATE OR REPLACE FUNCTION public.udf_updatecontact(
	_cid integer,
	_pid integer,
	_firstname character varying,
	_lastname character varying,
	_emailaddress character varying,
	_company character varying,
	_category character varying,
	_gender character varying,
	_dob timestamp without time zone,
	_modeslack boolean,
	_modewhatsapp boolean,
	_modeemail boolean,
	_modephone boolean,
	_contactimage character varying)
    RETURNS integer
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
BEGIN
	UPDATE public.tblcontacts
	SET professionid=_pid, 
	firstname=_firstname,
	lastname=_lastname, 
	emailaddress=_emailaddress,
	company=_company, category=_category::udt_category, gender=_gender::udt_gender, dob=_dob, 
	modeslack=_modeslack, modewhatsapp=_modewhatsapp, modeemail=_modeemail,
	modephone=_modephone, contactimage=_contactimage, lastmodified=default
	WHERE contactid = _cid;
	IF FOUND THEN -- UPDATED SUCCESSFULLY
	RETURN 1;
	ELSE RETURN 0; -- UPDATE FAIL
	END IF;																	
END
$BODY$;

-- FUNCTION: public.udf_updateprofession(integer, character varying, character varying)

-- DROP FUNCTION IF EXISTS public.udf_updateprofession(integer, character varying, character varying);

CREATE OR REPLACE FUNCTION public.udf_updateprofession(
	_pid integer,
	_profession character varying,
	_description character varying)
    RETURNS integer
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
BEGIN
	UPDATE public.tblprofessions
	SET profession = _profession, 
	description = _description,
	lastmodified = default
	WHERE professionid = _pid;
	IF FOUND THEN -- Updated SUCCESSFULLY
	RETURN 1;
	ELSE RETURN 0; -- Updated FAIL
	END IF;																	
END
$BODY$;