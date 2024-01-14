const { Type } = require("../db");
const axios = require("axios");

const getAllTypes = async (req, res) => {
  try {
    const typedb = await Type.findAll();

    if (typedb.length === 0) {
      const infoApi = (await axios.get("https://pokeapi.co/api/v2/type")).data.results;
    
      const typeRecords = infoApi.map((type, index) => ({ id: index + 1, name: type.name }));
    
      const typeDB = await Type.bulkCreate(typeRecords);
      
      return res.status(200).json(typeDB);
    }
    
    return res.status(200).json(typedb);
    
  } catch (error) {
    return res.status(200).json({ error: error.message });
  }
};


module.exports = getAllTypes