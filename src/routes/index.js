// Importar todos los routers;
// Ejemplo: const authRouter = require('./auth.js');
const getPokemons = require('../controllers/getPokemons');
const getPokemonById = require('../controllers/getPokemonById');
const getPokemonsName = require('../controllers/getPokemonsName');
const postPokemon = require('../controllers/postPokemon');
const getAllTypes = require('../controllers/getAllTypes')
const router = require('express').Router();

router.get('/pokemons', getPokemons)
router.get('/pokemons/name', getPokemonsName);
router.get('/pokemons/:id', getPokemonById)
router.post('/pokemons', postPokemon);
router.get('/types', getAllTypes);

module.exports = router;

// Configurar los routers
// Ejemplo: router.use('/auth', authRouter);


