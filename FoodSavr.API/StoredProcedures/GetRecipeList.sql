SELECT R.id, count(R.id) AS matches, R.name, R.description, R.imglink, R.portions, R.cookingtime AS time
  FROM recipeingredient AS RIL
  JOIN recipe AS R on RIL.recipeid = R.id
  JOIN ingredient AS I on I.id = RIL.ingredientid
  
  WHERE I.categoryid IN (
  	SELECT I2.categoryid
  		FROM ingredient AS I2
  		JOIN category AS C ON C.id = I2.categoryid
  		WHERE I2.id IN (1, 2, 3, 4, 26) -- add the necessary questionmark
  		AND C.Id <> 0 /*--Uncategorized */) 
  
  GROUP BY R.id
  ORDER BY count(R.id) DESC;