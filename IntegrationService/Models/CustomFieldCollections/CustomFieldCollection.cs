using System.Collections;

namespace IntegrationService.Models.CustomFieldCollections
{
	public class CustomFieldCollection<T> : IEnumerable<T>
	{
		private List<T> items = new List<T>();

		// Add items to the collection
		public void AddItem(T item)
		{
			items.Add(item);
		}

		// Implementation of IEnumerable.GetEnumerator()
		IEnumerator IEnumerable.GetEnumerator()
		{
			return items.GetEnumerator();
		}

		// Implementation of IEnumerable<T>.GetEnumerator()


		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
